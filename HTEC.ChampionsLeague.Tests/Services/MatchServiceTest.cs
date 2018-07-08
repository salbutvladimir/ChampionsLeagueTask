using HTEC.ChampionsLeague.Context;
using HTEC.ChampionsLeague.Dto;
using HTEC.ChampionsLeague.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTEC.ChampionsLeague.Tests.Services
{
    [TestClass]
    public class MatchServiceTest
    {
        private ApplicationContext _applicationContext;
        private MatchService _matchService;
        private TableService _tableService;
        private StandingService _standingService;

        private DbContextOptions<ApplicationContext> options;

        private List<Match> matches;
        private List<MatchDto> matchesDto;
        private List<MatchDto> matchesDto2;

        [TestInitialize]
        public void Initilaize()
        {
            MapperInitialize.Configure();

            options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDataBase").Options;
            _applicationContext = new ApplicationContext(options);
            _standingService = new StandingService(_applicationContext);
            _tableService = new TableService(_applicationContext, _standingService);
            _matchService = new MatchService(_applicationContext, _tableService);

            matches = new List<Match>()
            {
                new Match { Id = 1, LeagueTitle = "Test league", Matchday = 1, Group = "A",
                    HomeTeam = "Team 1", AwayTeam = "Team 2", KickoffAt = DateTime.Now, Score = "2:1" },
                new Match { Id = 2, LeagueTitle = "Test league", Matchday = 1, Group = "A",
                    HomeTeam = "Team 3", AwayTeam = "Team 4", KickoffAt = DateTime.Now, Score = "1:3" },
                new Match { Id = 3, LeagueTitle = "Test league", Matchday = 2, Group = "A",
                    HomeTeam = "Team 2", AwayTeam = "Team 3", KickoffAt = DateTime.Now, Score = "1:6" },
                new Match { Id = 4, LeagueTitle = "Test league", Matchday = 2, Group = "A",
                    HomeTeam = "Team 4", AwayTeam = "Team 1", KickoffAt = DateTime.Now.AddDays(-7), Score = "0:1" },
            };

            matchesDto = new List<MatchDto>()
            {
                new MatchDto { LeagueTitle = "Test league", Matchday = 1, Group = "A",
                    HomeTeam = "Team 1", AwayTeam = "Team 2", KickoffAt = DateTime.Now.ToString("s"), Score = "1:1" },
                new MatchDto { LeagueTitle = "Test league", Matchday = 1, Group = "A",
                    HomeTeam = "Team 3", AwayTeam = "Team 4", KickoffAt = DateTime.Now.ToString("s"), Score = "1:0" },
                new MatchDto { LeagueTitle = "Test league", Matchday = 2, Group = "A",
                    HomeTeam = "Team 2", AwayTeam = "Team 3", KickoffAt = DateTime.Now.ToString("s"), Score = "1:3" },
                new MatchDto { LeagueTitle = "Test league", Matchday = 2, Group = "A",
                    HomeTeam = "Team 4", AwayTeam = "Team 1", KickoffAt = DateTime.Now.ToString("s"), Score = "2:1" },
            };

            matchesDto2 = new List<MatchDto>()
            {
                new MatchDto { LeagueTitle = "Test league", Matchday = 1, Group = "A",
                    HomeTeam = "Team 1", AwayTeam = "Team 2", KickoffAt = DateTime.Now.ToString("s"), Score = "2:1" },
                new MatchDto { LeagueTitle = "Test league", Matchday = 1, Group = "A",
                    HomeTeam = "Team 3", AwayTeam = "Team 4", KickoffAt = DateTime.Now.ToString("s"), Score = "1:1" },
                new MatchDto { LeagueTitle = "Test league", Matchday = 2, Group = "A",
                    HomeTeam = "Team 2", AwayTeam = "Team 3", KickoffAt = DateTime.Now.ToString("s"), Score = "0:1" },
                new MatchDto { LeagueTitle = "Test league", Matchday = 2, Group = "A",
                    HomeTeam = "Team 4", AwayTeam = "Team 1", KickoffAt = DateTime.Now.ToString("s"), Score = "2:2" },
            };
        }

        [TestMethod]
        public void TestGetMatches_WithEmptyFilter()
        {
            _applicationContext.Database.EnsureDeleted();
            _applicationContext.Matches.AddRange(this.matches);
            _applicationContext.SaveChanges();

            var filter = new MatchFilterDto();
            var matches = _matchService.GetMatches(filter);

            Assert.AreEqual(4, matches.Count);
        }

        [TestMethod]
        public void TestGetMatches_WithFilter()
        {
            _applicationContext.Database.EnsureDeleted();
            _applicationContext.Matches.AddRange(this.matches);
            _applicationContext.SaveChanges();

            var filter = new MatchFilterDto
            {
                DateFrom = DateTime.Now.AddDays(-3),
                DateTo = DateTime.Now.AddDays(1),
                Group = new string[] { "A" },
                Team = new string[] { "Team 1", "Team 2" }
            };

            var matches = _matchService.GetMatches(filter);

            Assert.AreEqual(2, matches.Count);
        }

        [TestMethod]
        public async Task TestAddOrUpdateMatches_AddMatches()
        {
            _applicationContext.Database.EnsureDeleted();

            var tables = await _matchService.AddOrUpdateMatches(matchesDto);
            var matches = _applicationContext.Matches.ToList();

            Assert.AreEqual(4, matches.Count);

            Assert.AreEqual(1, tables.Count);
            Assert.AreEqual("Team 3", tables[0].Standing[0].Team);
            Assert.AreEqual(6, tables[0].Standing[0].Points);
            Assert.AreEqual(1, tables[0].Standing[0].Rank);
            Assert.AreEqual(4, tables[0].Standing[0].Goals);
            Assert.AreEqual(1, tables[0].Standing[0].GoalsAgainst);
            Assert.AreEqual(3, tables[0].Standing[0].GoalDifference);
            Assert.AreEqual(2, tables[0].Standing[0].Win);
            Assert.AreEqual(0, tables[0].Standing[0].Lose);
            Assert.AreEqual(0, tables[0].Standing[0].Draw);
        }

        [TestMethod]
        public async Task TestAddOrUpdateMatches_UpdateMatches()
        {
            _applicationContext.Database.EnsureDeleted();

            var tables = await _matchService.AddOrUpdateMatches(matchesDto);
            var updatedTables = await _matchService.AddOrUpdateMatches(matchesDto2);

            var matches = _applicationContext.Matches.ToList();

            Assert.AreEqual(4, matches.Count);

            Assert.AreEqual(1, tables.Count);
            Assert.AreEqual("Team 3", tables[0].Standing[0].Team);
            Assert.AreEqual(6, tables[0].Standing[0].Points);
            Assert.AreEqual(1, tables[0].Standing[0].Rank);
            Assert.AreEqual(4, tables[0].Standing[0].Goals);
            Assert.AreEqual(1, tables[0].Standing[0].GoalsAgainst);
            Assert.AreEqual(3, tables[0].Standing[0].GoalDifference);
            Assert.AreEqual(2, tables[0].Standing[0].Win);
            Assert.AreEqual(0, tables[0].Standing[0].Lose);
            Assert.AreEqual(0, tables[0].Standing[0].Draw);

            Assert.AreEqual(1, updatedTables.Count);
            Assert.AreEqual("Team 1", updatedTables[0].Standing[0].Team);
            Assert.AreEqual(4, updatedTables[0].Standing[0].Points);
            Assert.AreEqual(1, updatedTables[0].Standing[0].Rank);
            Assert.AreEqual(4, updatedTables[0].Standing[0].Goals);
            Assert.AreEqual(3, updatedTables[0].Standing[0].GoalsAgainst);
            Assert.AreEqual(1, updatedTables[0].Standing[0].GoalDifference);
            Assert.AreEqual(1, updatedTables[0].Standing[0].Win);
            Assert.AreEqual(0, updatedTables[0].Standing[0].Lose);
            Assert.AreEqual(1, updatedTables[0].Standing[0].Draw);
        }
    }
}
