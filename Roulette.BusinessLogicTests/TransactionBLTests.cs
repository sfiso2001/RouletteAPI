using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Roulette.BusinessLogic;
using Roulette.BusinessLogic.DTO.Requests;
using Roulette.DataAccess.Interfaces;
using Roulette.Models;

namespace Roulette.BusinessLogicTests
{
    [TestClass]
    public class TransactionBLTests : BaseTest
    {
        private readonly Mock<IGameTransactionRepository> _gameTransactionRepository = new();
        private readonly Mock<IPlayerDetailRepository> _playerDetailRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private Mock<ILogger<TransactionsBL>> _logger = new();
        private TransactionsBL _transactionBL;

        [TestInitialize]
        public void Initialize()
        {
            _transactionBL = new TransactionsBL(_unitOfWork.Object, _logger.Object);
        }
        
        [TestMethod]
        public async Task GetPlayerBalance_Success()
        {
            // given
            var playerDetail = new PlayerDetail()
            {
                Id = _faker.Random.Int(),
                PlayerName = _faker.Random.String2(FAKER_STRING2_LENGTH),
                Balance = _faker.Random.Int(),
            };

            bool playerDetailRepositoryGetAsyncHasBeenCalled = false;
            _playerDetailRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>()))
                .Callback(() => playerDetailRepositoryGetAsyncHasBeenCalled = true)
                .Returns(Task.FromResult(playerDetail));

            var playerBalanceRequest = new PlayerBalanceRequest()
            {
                PlayerId = playerDetail.Id                
            };

            // when
            var result = await _transactionBL.PlayerBalanceAsync(playerBalanceRequest);

            // then
            Assert.IsNotNull(result);
            playerDetailRepositoryGetAsyncHasBeenCalled.Should().BeTrue();
        }             
    }
}