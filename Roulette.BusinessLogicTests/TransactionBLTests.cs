using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Roulette.BusinessLogic;
using Roulette.BusinessLogic.DTO.Requests;
using Roulette.Common.Enums;
using Roulette.DataAccess.Interfaces;
using Roulette.Models;

namespace Roulette.BusinessLogicTests
{
    [TestClass]
    public class TransactionBLTests : BaseTest
    {
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

            bool playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = false;
            _unitOfWork.Setup(x => x.PlayerDetailRepository.GetAsync(It.IsAny<int>()))
                .Callback(() => playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = true)
                .Returns(Task.FromResult(playerDetail));

            var playerBalanceRequest = new PlayerBalanceRequest()
            {
                PlayerId = playerDetail.Id                
            };

            // when
            var result = await _transactionBL.PlayerBalanceAsync(playerBalanceRequest);

            // then
            Assert.IsNotNull(result);
            playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled.Should().BeTrue();
        }

        [TestMethod]
        public async Task PlaceBet_With_Valid_Details_Should_Return_Success_True()
        {
            // given
            var playerDetail = new PlayerDetail()
            {
                Id = _faker.Random.Int(),
                PlayerName = _faker.Random.String2(FAKER_STRING2_LENGTH),
                Balance = 20,
            };

            bool playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = false;
            _unitOfWork.Setup(x => x.PlayerDetailRepository.GetAsync(It.IsAny<int>()))
                .Callback(() => playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = true)
                .Returns(Task.FromResult(playerDetail));

            bool gameTransactionAddHasBeenCalled = false;
            _unitOfWork.Setup(x => x.GameTransactionRepository.Add(It.IsAny<GameTransaction>()))
                .Callback(() => gameTransactionAddHasBeenCalled = true);

            bool unitOfWorkSaveHasBeenCalled = false;
            _unitOfWork.Setup(x => x.Save())
                .Callback(() => unitOfWorkSaveHasBeenCalled = true);

            var placeBetRequest = new PlaceBetRequest()
            {
                PlayerId = playerDetail.Id,
                GameId = _faker.Random.String2(10),
                Reference = _faker.Random.String2(20),
                StakeAmount = 10
            };

            // when
            var result = await _transactionBL.DebitTransactionAsync(placeBetRequest);

            // then
            Assert.IsNotNull(result);
            playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled.Should().BeTrue();
            gameTransactionAddHasBeenCalled.Should().BeTrue();
            unitOfWorkSaveHasBeenCalled.Should().BeTrue();
            result.Success.Should().BeTrue();            
        }

        [TestMethod]
        public async Task PlaceBet_With_Low_Balance_Should_Return_Success_False()
        {
            // given
            var playerDetail = new PlayerDetail()
            {
                Id = _faker.Random.Int(),
                PlayerName = _faker.Random.String2(FAKER_STRING2_LENGTH),
                Balance = 0,
            };

            bool playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = false;
            _unitOfWork.Setup(x => x.PlayerDetailRepository.GetAsync(It.IsAny<int>()))
                .Callback(() => playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = true)
                .Returns(Task.FromResult(playerDetail));

            var placeBetRequest = new PlaceBetRequest()
            {
                PlayerId = playerDetail.Id,
                GameId = _faker.Random.String2(10),
                Reference = _faker.Random.String2(20),
                StakeAmount = 10
            };

            // when
            var result = await _transactionBL.DebitTransactionAsync(placeBetRequest);

            // then
            Assert.IsNotNull(result);
            playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled.Should().BeTrue();
            result.Success.Should().BeFalse();
        }

        [TestMethod]
        public async Task Spin_With_No_Initial_Bet_Should_Return_Success_False()
        {
            // given
            IEnumerable<GameTransaction> gameTransactions = new List<GameTransaction>();

            bool gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled = false;
            _unitOfWork.Setup(x => x.GameTransactionRepository.GameTransactionBetsByReference(It.IsAny<string>()))
                .Callback(() => gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled = true)
                .Returns(Task.FromResult(gameTransactions));

            var spinRequest = new SpinRequest()
            {
                PlayerId = It.IsAny<int>(),
                GameId = _faker.Random.String2(10),
                Reference = _faker.Random.String2(20),
                WinAmount = 10
            };

            // when
            var result = await _transactionBL.PlaySpinAsync(spinRequest);

            // then
            Assert.IsNotNull(result);
            gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled.Should().BeTrue();
            result.Success.Should().BeFalse();
        }

        [TestMethod]
        public async Task Spin_With_Valid_Details_Should_Return_Success()
        {
            // given
            var spinRequest = new SpinRequest()
            {
                PlayerId = It.IsAny<int>(),
                GameId = _faker.Random.String2(10),
                Reference = _faker.Random.String2(20),
                WinAmount = 10
            };

            GameTransaction gameTransaction = new GameTransaction(
                transactionType: TransactionType.Bet.ToString(),
                gameId: _faker.Random.String2(2),
                reference: spinRequest.Reference,
                spinReference: spinRequest.Reference,
                playerId: spinRequest.PlayerId,
                stakeAmount: 10,
                outcomeAmount: 0,
                createdDate: DateTime.Now);

            IEnumerable<GameTransaction> gameTransactions = new List<GameTransaction>()
            {
                gameTransaction
            };

            bool gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled = false;
            _unitOfWork.Setup(x => x.GameTransactionRepository.GameTransactionBetsByReference(It.IsAny<string>()))
                .Callback(() => gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled = true)
                .Returns(Task.FromResult(gameTransactions));

            bool gameTransactionRepositoryGameTransactionGetAsyncHasBeenCalled = false;
            _unitOfWork.Setup(x => x.GameTransactionRepository.GetAsync(It.IsAny<int>()))
                .Callback(() => gameTransactionRepositoryGameTransactionGetAsyncHasBeenCalled = true)
                .Returns(Task.FromResult(gameTransaction));
            // when
            var result = await _transactionBL.PlaySpinAsync(spinRequest);

            // then
            Assert.IsNotNull(result);
            gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled.Should().BeTrue();
            gameTransactionRepositoryGameTransactionGetAsyncHasBeenCalled.Should().BeTrue();
            result.Success.Should().BeTrue();
        }
    }
}