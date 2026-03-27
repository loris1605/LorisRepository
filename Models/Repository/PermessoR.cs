using Models.Context;
using Models.Tables;
using System.Diagnostics;

namespace Models.Repository
{
    public class PermessoR : BaseRepository<AppDbContext, Permesso>
    {
        private static int classCount;

        public PermessoR() : base()
        {
            System.Diagnostics.Debug.WriteLine($"***** [VM] {this.GetType().Name} " +
                                               $"#{Interlocked.Increment(ref classCount)} caricato *****");
        }

#if DEBUG

        ~PermessoR()
        {
            // Questo apparirà nella finestra "Output" di Visual Studio
            Debug.WriteLine($"***** [GC] {this.GetType().Name} " +
                            $"#{classCount} DISTRUTTO *****");
        }

#endif

        
    }
}
