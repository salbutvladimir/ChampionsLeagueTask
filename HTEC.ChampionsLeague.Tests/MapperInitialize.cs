using AutoMapper;

namespace HTEC.ChampionsLeague.Tests
{
    public class MapperInitialize
    {
        private static readonly object Sync = new object();
        private static bool _configured;

        public static void Configure()
        {
            lock (Sync)
            {
                if (!_configured)
                    //Mapper.Reset();
                    Mapper.Initialize(cfg =>
                    {
                        cfg.AddProfile(new EntityMapper());
                    });
                _configured = true;
            }
        }
    }
}
