using Microsoft.EntityFrameworkCore;
using Models.Context;
using Models.Entity;
using Models.Interfaces;
using Models.Projections;
using Models.Tables;
using System.Data;
using System.Diagnostics;

namespace Models.Repository
{
    
    public partial class PersonR : IDisposable, IGroupQ<PersonMap>
    {
        private static int classCount;
        private readonly PeopleDbContext _ctx;

        public PersonR() : base()
        {
            System.Diagnostics.Debug.WriteLine($"***** [VM] {this.GetType().Name} " +
                                               $"#{Interlocked.Increment(ref classCount)} " +
                                               $"caricato *****");

            _ctx = new PeopleDbContext();
        }

#if DEBUG

        ~PersonR()
        {
            // Questo apparirà nella finestra "Output" di Visual Studio
            Debug.WriteLine($"***** [GC] {this.GetType().Name} " +
                            $"#{classCount} DISTRUTTO *****");
        }

#endif

        public virtual void Dispose()
        {
            _ctx.Dispose();
        }
        
        public Task<List<PersonMap>> LoadByModel(object model) => 
                Task.FromResult((List<PersonMap>)model);
        
        public async Task<List<PersonMap>> Load(int id)
        {
            IQueryable<Person> query = _ctx.People.AsNoTracking();

            if (id > 0)
                query = query.Where(p => p.Id == id);
            else
                query = query.Where(p => p.Id > 0).Take(100);

            return await query
                    .SelectMany(
                        person => person.Soci.DefaultIfEmpty(),
                        (person, socio) => new { person, socio }
                    )
                    .SelectMany(
                        combined => (combined.socio!.Tessere.DefaultIfEmpty()),

                        (combined, tessera) => 
                        PersonProjections.PeopleToPersonMap(combined.person, combined.socio, tessera))
                    .Take(100)
                    .ToListAsync();

           

        }

        public async Task<PersonMap> FirstPerson(int id)
        {
            // Usiamo la proprietà _context definita a livello di classe
            var result = await _ctx.People
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(PersonProjections.ToSimplePersonMap)
                .FirstOrDefaultAsync();

            // Se result è null, restituisce una nuova istanza vuota (come nel tuo codice originale)
            return result ?? new PersonMap();
        }
        public async Task<PersonMap> FirstSocio(int idSocio)
        {
            var result = await _ctx.Soci
                .AsNoTracking()
                .Where(s => s.Id == idSocio)
                // Partiamo dal Socio (s), carichiamo la Persona (s.Person) 
                // e le Tessere (s.Tessere) con LEFT JOIN
                .Select(s => PersonProjections.PeopleToPersonMap(s.Person!, s, null))
                .FirstOrDefaultAsync();

            // Se non trova il socio, restituisce un oggetto vuoto per il binding
            return result ?? new PersonMap();
        }
        public async Task<PersonMap> FirstTessera(int idTessera)
        {
            var result = await _ctx.Tessere
                .AsNoTracking()
                .Where(t => t.Id == idTessera)
                // Risaliamo alla Persona tramite il Socio (t.Socio.Person)
                // Passiamo: (Persona, Socio, Tessera)
                .Select(t => PersonProjections.PeopleToPersonMap(t.Socio!.Person!, t.Socio, t))
                .FirstOrDefaultAsync();

            // Ritorna il record completo (Persona + Socio + Dati di QUESTA tessera)
            return result ?? new PersonMap();
        }

        public async Task<bool> HasCodiciSocio(int idperson)
        {
            return await _ctx.Soci.AnyAsync(s => s.PersonId == idperson);
        }

        public async Task<int> Add(PersonMap map)
        {
            // 1. Creiamo l'albero degli oggetti collegati
            var person = new Person
            {
                FirstName = map.Nome,
                SurName = map.Cognome,
                Natoil = map.Natoil,
                UniqueParam = map.CodiceUnivoco,

                // Colleghiamo il Socio direttamente alla lista della Persona
                Soci =
                [
                    new Socio
                    {
                        NumeroSocio = map.NumeroSocio,
                        // Colleghiamo la Tessera direttamente al Socio
                        Tessere =
                        [
                            new Tessera
                            {
                                NumeroTessera = map.NumeroTessera,
                                Scadenza = map.Scadenza
                            }
                        ]
                    }
                ]
            };

            // 2. Aggiungiamo solo la "radice" (Person). EF aggiungerà i figli a cascata.
            _ctx.People.Add(person);

            try
            {
                // 3. Un'unica chiamata al database (Transazione atomica)
                await _ctx.SaveChangesAsync();

                // Dopo SaveChanges, person.Id contiene l'ID reale generato dal DB
                return person.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errore Add: {ex.InnerException?.Message ?? ex.Message}");
                return -1;
            }
        }
        public async Task<int> AddCodiceSocio(PersonMap map)
        {
            var socio = new Socio
            {
                NumeroSocio = map.NumeroSocio,
                PersonId = map.Id, // Assumiamo che map.Id sia già valorizzato con l'ID della Person esistente
                // Colleghiamo la Tessera direttamente al Socio
                Tessere =
                [
                    new Tessera
                    {
                        NumeroTessera = map.NumeroTessera,
                        Scadenza = map.Scadenza
                    }
                ]

            };

            // 2. Aggiungiamo solo la "radice" (Person). EF aggiungerà i figli a cascata.
            _ctx.Soci.Add(socio);

            try
            {
                // 3. Un'unica chiamata al database (Transazione atomica)
                await _ctx.SaveChangesAsync();

                // Dopo SaveChanges, person.Id contiene l'ID reale generato dal DB
                return socio.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errore Add: {ex.InnerException?.Message ?? ex.Message}");
                return -1;
            }
        }
        public async Task<int> AddTessera(PersonMap map)
        {
            var tessera = new Tessera
            {
                NumeroTessera = map.NumeroTessera,
                SocioId = map.CodiceSocio,
                Abilitato = true
                
            };

            // 2. Aggiungiamo solo la "radice" (Person). EF aggiungerà i figli a cascata.
            _ctx.Tessere.Add(tessera);

            try
            {
                // 3. Un'unica chiamata al database (Transazione atomica)
                await _ctx.SaveChangesAsync();

                // Dopo SaveChanges, person.Id contiene l'ID reale generato dal DB
                return tessera.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errore Add: {ex.InnerException?.Message ?? ex.Message}");
                return -1;
            }
        }

        //public bool Del(int id) => Del(id, sp.PersonDeleteData);

        public async Task<bool> Upd(PersonMap map)
        {
            // 1. Cerchiamo l'entità (FindAsync è ottimo qui)
            var person = await _ctx.People.FindAsync(map.Id);
            if (person == null) return false;
            // 2. Aggiorniamo le proprietà
            person.FirstName = map.Nome;
            person.SurName = map.Cognome;
            person.Natoil = map.Natoil;
            person.UniqueParam = map.CodiceUnivoco;

            try
            {
                // 3. FONDAMENTALE: Salva le modifiche effettive nel DB
                await _ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Logga l'errore (es. violazione di vincoli o database offline)
                Debug.WriteLine($"Errore Update: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> UpdSocio(PersonMap map)
        {
            // 1. Cerchiamo l'entità (FindAsync è ottimo qui)
            var socio = await _ctx.Soci.FindAsync(map.CodiceSocio);
            if (socio == null) return false;
            // 2. Aggiorniamo le proprietà
            socio.NumeroSocio = map.NumeroSocio;
            
            try
            {
                // 3. FONDAMENTALE: Salva le modifiche effettive nel DB
                await _ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Logga l'errore (es. violazione di vincoli o database offline)
                Debug.WriteLine($"Errore Update: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> UpdTessera(PersonMap map)
        {
            // 1. Cerchiamo l'entità (FindAsync è ottimo qui)
            var tessera = await _ctx.Tessere.FindAsync(map.CodiceTessera);
            if (tessera == null) return false;
            // 2. Aggiorniamo le proprietà
            tessera.NumeroTessera = map.NumeroTessera;

            try
            {
                // 3. FONDAMENTALE: Salva le modifiche effettive nel DB
                await _ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Logga l'errore (es. violazione di vincoli o database offline)
                Debug.WriteLine($"Errore Update: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> Del(PersonMap map)
        {
            var person = await _ctx.People.FindAsync(map.Id);
            if (person == null) return false;

            _ctx.People.Remove(person);

            try
            {
                // 3. Applichiamo la modifica al DB
                await _ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errore Delete: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DelSocio(PersonMap map)
        {
            var socio = await _ctx.Soci.FindAsync(map.CodiceSocio);
            if (socio == null) return false;

            _ctx.Soci.Remove(socio);

            try
            {
                // 3. Applichiamo la modifica al DB
                await _ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errore Delete: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DelTessera(PersonMap map)
        {
            var tessera = await _ctx.Tessere.FindAsync(map.CodiceTessera);
            if (tessera == null) return false;

            _ctx.Tessere.Remove(tessera);

            try
            {
                // 3. Applichiamo la modifica al DB
                await _ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errore Delete: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EsisteCodiceUnivoco(string codiceunivoco)
        {
            return await _ctx.People.AnyAsync(p => p.UniqueParam == codiceunivoco);
        }
        public async Task<bool> EsisteCodiceUnivoco(string codiceunivoco, int id)
        {
            return await _ctx.People.AnyAsync(p => p.UniqueParam == codiceunivoco && p.Id != id);

        }

        public async Task<bool> EsisteNumeroTessera(string numeroTessera)
        {
            return await _ctx.Tessere.AnyAsync(t => t.NumeroTessera == numeroTessera);
        }
        public async Task<bool> EsisteNumeroTesseraUpd(PersonMap dT)
        {
            return await _ctx.Tessere.AnyAsync(t => t.NumeroTessera == dT.NumeroTessera &&
                                                    t.Id != dT.CodiceTessera);
        }

        public async Task<bool> EsisteNumeroSocio(string numeroSocio)
        {
            return await _ctx.Soci.AnyAsync(s => s.NumeroSocio == numeroSocio);
        }
        public async Task<bool> EsisteNumeroSocioUpd(PersonMap dT)
        {
            return await _ctx.Soci.AnyAsync(s => s.NumeroSocio == dT.NumeroSocio &&
                                                 s.Id != dT.CodiceSocio);
        }


    }


}
