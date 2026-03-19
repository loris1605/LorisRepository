using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysNet.Converters
{
    public static class Converters
    {
        public static bool IsLegalAge(this int dataNascita)
        {
            // Validazione base del formato YYYYMMDD
            if (dataNascita < 19000101 || dataNascita > 21001231) return false;

            // Calcolo età (assumendo che DateIntToEta restituisca un int)
            int eta = dataNascita.DateIntToEta();

            // Restituisce true se maggiorenne (>= 18)
            return eta >= 18;
        }

        public static int DateIntToEta(this int dataNascita)
        {
            // Validazione rapida
            if (dataNascita < 19000101 || dataNascita > 21001231) return 0;

            try
            {
                // Estrazione matematica (molto più veloce di Substring)
                int annoNascita = dataNascita / 10000;
                int meseNascita = (dataNascita % 10000) / 100;
                int giornoNascita = dataNascita % 100;

                DateTime oggi = DateTime.Today;
                int eta = oggi.Year - annoNascita;

                // Sottrai un anno se il compleanno non è ancora avvenuto quest'anno
                if (oggi.Month < meseNascita || (oggi.Month == meseNascita && oggi.Day < giornoNascita))
                {
                    eta--;
                }

                return eta < 0 ? 0 : eta;
            }
            catch
            {
                return 0; // Fallback sicuro in caso di data non valida (es. 31 aprile)
            }
        }

        public static DateTime DateIntToDate(this int intData)
        {
            // Validazione immediata (formato YYYYMMDD deve avere 8 cifre)
            if (intData < 19000101 || intData > 21001231) return DateTime.MinValue;

            // Estraiamo i numeri direttamente senza passare dalle stringhe (più veloce)
            int year = intData / 10000;
            int month = (intData % 10000) / 100;
            int day = intData % 100;

            try
            {
                // DateTime constructor è più veloce di TryParse
                return new DateTime(year, month, day);
            }
            catch
            {
                // Se il numero è 20240230 (30 febbraio), il costruttore fallisce
                return DateTime.MinValue;
            }
        }



        public static int DateTimeToIntDate(this DateTime date)
        {
            // Se la data è MinValue o non valida per il tuo range, restituisce 0
            if (date == DateTime.MinValue || date.Year < 1900 || date.Year > 2100) return 0;

            // Esempio: 2024 * 10000 + 5 * 100 + 20 = 20240520
            return (date.Year * 10000) + (date.Month * 100) + date.Day;
        }

        public static int DateTimeToIntDate(this DateTime? date)
        {
            // Se l'oggetto è null, restituisce 0
            if (!date.HasValue) return 0;

            return date.Value.DateTimeToIntDate();
        }

        public static DateTimeOffset DateIntToDateTimeOffset(this int intData)
        {
            // Validazione immediata del formato YYYYMMDD
            if (intData < 19000101 || intData > 21001231) return DateTimeOffset.MinValue;

            // Estrazione matematica (evita allocazioni di stringhe)
            int year = intData / 10000;
            int month = (intData % 10000) / 100;
            int day = intData % 100;

            try
            {
                // Il costruttore richiede (anno, mese, giorno, ora, min, sec, offset)
                // Usiamo TimeSpan.Zero per UTC o l'offset locale corrente
                return new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
            }
            catch
            {
                // Fallback per date non valide (es. 20230230)
                return DateTimeOffset.MinValue;
            }
        }

        public static int DateTimeOffsetToDateInt(this DateTimeOffset dto)
        {
            // Se è il valore minimo o fuori dal range gestito dal tuo DB
            if (dto == DateTimeOffset.MinValue || dto.Year < 1900 || dto.Year > 2100) return 0;

            // Calcolo matematico diretto: YYYY * 10000 + MM * 100 + DD
            return (dto.Year * 10000) + (dto.Month * 100) + dto.Day;
        }

        public static int DateTimeOffsetToDateInt(this DateTimeOffset? dto)
        {
            // Se l'oggetto è null, restituiamo 0 (coerente con il tuo sistema)
            if (!dto.HasValue) return 0;

            // Chiamata al metodo sopra tramite .Value
            return dto.Value.DateTimeOffsetToDateInt();
        }


    }

    public static class  Services
    {
        public static void FireAndForget(this Task task)
        {
            task.ContinueWith(t => {
                if (t.IsFaulted) { /* Logga l'errore qui */ }
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
