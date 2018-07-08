using HTEC.ChampionsLeague.Context;
using HTEC.ChampionsLeague.Dto;
using HTEC.ChampionsLeague.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTEC.ChampionsLeague.Tests.Services
{
    [TestClass]
    public class TableServiceTest
    {
        private ApplicationContext _applicationContext;
        private TableService _tableService;
        private StandingService _standingService;
        
        private DbContextOptions<ApplicationContext> options;

        private List<Table> tables;
        private List<Match> matches;
        private MatchDto matchDto;
        private MatchDto matchDto2;
        
        [TestInitialize]
        public void Initialize()
        {
            MapperInitialize.Configure();

            options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDataBase").Options;
            _applicationContext = new ApplicationContext(options);
            _standingService = new StandingService(_applicationContext);
            _tableService = new TableService(_applicationContext, _standingService);

            tables = new List<Table>
            {
                new Table
                {
                    Id = 1, LeagueTitle = "Test league", Group = "A", Matchday = 1,
                    Standing = new List<Standing>
                    {
                        new Standing{ Id = 1, Team = "Team 1", PlayedGames = 1, Points = 3, Goals = 3,
                            GoalsAgainst = 1, GoalDifference = 2, Win = 1, Lose = 0, Draw = 0},
                        new Standing { Id = 2, Team = "Team 2", PlayedGames = 1, Points = 0, Goals = 1,
                            GoalsAgainst = 3, GoalDifference = -2, Win = 0, Lose = 1, Draw = 0 }
                    }
                },
                new Table()
                {
                    Id = 2, LeagueTitle = "Test league", Group = "B", Matchday = 1,
                    Standing = new List<Standing>
                    {
                        new Standing { Id = 3, Team = "Team 3", PlayedGames = 1, Points = 1, Goals = 1,
                            GoalsAgainst = 1, GoalDifference = 0, Win = 0, Lose = 0, Draw = 1 },
                        new Standing { Id = 4, Team = "Team 4", PlayedGames = 1, Points = 1, Goals = 1,
                            GoalsAgainst = 1, GoalDifference = 0, Win = 0, Lose = 1, Draw = 1 }
                    }
                }
            };

            matches = new List<Match>()
            {
                new Match { Id = 1, LeagueTitle = "Test league", Matchday = 1, Group = "A",
                    HomeTeam = "Team 1", AwayTeam = "Team 2", KickoffAt = DateTime.Now, Score = "3:1" },
                new Match { Id = 2, LeagueTitle = "Test league", Matchday = 1, Group = "B",
                    HomeTeam = "Team 3", AwayTeam = "Team 4", KickoffAt = DateTime.Now, Score = "1:1" },
            };

            matchDto = new MatchDto
            {
                LeagueTitle = "Test league",
                Matchday = 1,
                Group = "B",
                HomeTeam = "Team 3",
                AwayTeam = "Team 4",
                KickoffAt = DateTime.Now.ToString("s"),
                Score = "1:1"
            };

            matchDto2 = new MatchDto
            {
                LeagueTitle = "Test league",
                Matchday = 1,
                Group = "B",
                HomeTeam = "Team 3",
                AwayTeam = "Team 4",
                KickoffAt = DateTime.Now.ToString("s"),
                Score = "1:3"
            };

        }

        [TestMethod]
        public async Task TestGetTables_WithEmptyFilter()
        {
            _applicationContext.Database.EnsureDeleted();
            _applicationContext.Matches.AddRange(this.matches);
            _applicationContext.Tables.AddRange(this.tables);
            _applicationContext.SaveChanges();

            MapperInitialize.Configure();

            var filter = new TableFilterDto();
            var tables = await _tableService.GetTables(filter);

            Assert.AreEqual(2, tables.Count);
            Assert.AreEqual(2, tables[0].Standing.Count);
        }

        [TestMethod]
        public async Task TestGetTables_WithFilter()
        {
            _applicationContext.Database.EnsureDeleted();
            _applicationContext.Matches.AddRange(this.matches);
            _applicationContext.Tables.AddRange(this.tables);
            _applicationContext.SaveChanges();

            MapperInitialize.Configure();

            var filter = new TableFilterDto
            {
                Group = new string[] { "B" }
            };
            var tables = await _tableService.GetTables(filter);

            Assert.AreEqual(1, tables.Count);
            Assert.AreEqual("Team 3", tables[0].Standing[0].Team);
        }
    }
}