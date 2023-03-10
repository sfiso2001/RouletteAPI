using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Roulette.BusinessLogicTests;
using Roulette.BusinessLogic.DTO.Requests;
using Roulette.Common.Enums;
using Roulette.DataAccess.Interfaces;
using Roulette.Models;

namespace Roulette.BusinessLogic.Tests
{
    [TestClass]
    public class TransactionBlTests : BaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<ILogger<TransactionsBL>> _logger = new();
        private TransactionsBL _transactionBl;

        [TestInitialize]
        public void Initialize()
        {
            _transactionBl = new TransactionsBL(_unitOfWork.Object, _logger.Object);
        }
        
        [TestMethod]
        public async Task GetPlayerBalance_With_Valid_Player_details_Returns_Success_True()
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
            var result = await _transactionBl.PlayerBalanceAsync(playerBalanceRequest);

            // then
            Assert.IsNotNull(result);
            playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled.Should().BeTrue();
        }

        [TestMethod]
        public async Task GetPlayerBalance_With_No_Valid_Player_Return_Success_False()
        {
            // given
            PlayerDetail playerDetail = null!;

            bool playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = false;
            _unitOfWork.Setup(x => x.PlayerDetailRepository.GetAsync(It.IsAny<int>()))
                .Callback(() => playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = true)
                .Returns(Task.FromResult(playerDetail));

            var playerBalanceRequest = new PlayerBalanceRequest()
            {
                PlayerId = _faker.Random.Int()
            };

            // when
            var result = await _transactionBl.PlayerBalanceAsync(playerBalanceRequest);

            // then
            Assert.IsNotNull(result);
            playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled.Should().BeTrue();
            result.Success.Should().BeFalse();

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
            var result = await _transactionBl.DebitTransactionAsync(placeBetRequest);

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
            var result = await _transactionBl.DebitTransactionAsync(placeBetRequest);

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
            _unitOfWork.Setup(x => x.GameTransactionRepository.GameTransactionBetsByReferenceAsync(It.IsAny<string>()))
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
            var result = await _transactionBl.PlaySpinAsync(spinRequest);

            // then
            Assert.IsNotNull(result);
            gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled.Should().BeTrue();
            result.Success.Should().BeFalse();
        }

        [TestMethod]
        public async Task Spin_With_Valid_Details_Should_Return_Success_True()
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
            _unitOfWork.Setup(x => x.GameTransactionRepository.GameTransactionBetsByReferenceAsync(It.IsAny<string>()))
                .Callback(() => gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled = true)
                .Returns(Task.FromResult(gameTransactions));

            bool gameTransactionRepositoryGameTransactionGetAsyncHasBeenCalled = false;
            _unitOfWork.Setup(x => x.GameTransactionRepository.GetAsync(It.IsAny<int>()))
                .Callback(() => gameTransactionRepositoryGameTransactionGetAsyncHasBeenCalled = true)
                .Returns(Task.FromResult(gameTransaction));
            // when
            var result = await _transactionBl.PlaySpinAsync(spinRequest);

            // then
            Assert.IsNotNull(result);
            gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled.Should().BeTrue();
            gameTransactionRepositoryGameTransactionGetAsyncHasBeenCalled.Should().BeTrue();
            result.Success.Should().BeTrue();
        }

        [TestMethod]
        public async Task CreditPlayer_With_Valid_Details_Should_Return_Success_True()
        {
            // when
            PayoutRequest payoutRequest = new PayoutRequest()
            {
                GameId = _faker.Random.String2(4),
                PlayerId = _faker.Random.Int(),
                Reference = _faker.Random.String2(10)
            };

            GameTransaction gameTransaction = new GameTransaction(
                transactionType: TransactionType.Bet.ToString(),
                gameId: _faker.Random.String2(2),
                reference: payoutRequest.Reference,
                spinReference: payoutRequest.Reference,
                playerId: payoutRequest.PlayerId,
                stakeAmount: 10,
                outcomeAmount: 5000.00,
                createdDate: DateTime.Now);

            IEnumerable<GameTransaction> gameTransactions = new List<GameTransaction>()
            {
                gameTransaction
            };

            var playerDetail = new PlayerDetail()
            {
                Id = _faker.Random.Int(),
                PlayerName = _faker.Random.String2(FAKER_STRING2_LENGTH),
                Balance = 2000,
            };

            bool playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = false;
            _unitOfWork.Setup(x => x.PlayerDetailRepository.GetAsync(It.IsAny<int>()))
                .Callback(() => playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = true)
                .Returns(Task.FromResult(playerDetail));

            bool gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled = false;
            _unitOfWork.Setup(x => x.GameTransactionRepository.GameTransactionBetsByReferenceAsync(gameTransaction.Reference))
                .Callback(() => gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled = true)
                .Returns(Task.FromResult(gameTransactions));

            bool gameTransactionRepositoryGetGameTransactionPayoutAsyncHasBeenCalled = false;
            _unitOfWork.Setup(x => x.GameTransactionRepository.GetGameTransactionPayoutAsync(gameTransaction.Reference))
                .Callback(() => gameTransactionRepositoryGetGameTransactionPayoutAsyncHasBeenCalled = true)
                .Returns(Task.FromResult(5000.00));

            bool gameTransactionRepositoryGameTransactionGetAsyncHasBeenCalled = false;
            _unitOfWork.Setup(x => x.GameTransactionRepository.GetAsync(It.IsAny<int>()))
                .Callback(() => gameTransactionRepositoryGameTransactionGetAsyncHasBeenCalled = true)
                .Returns(Task.FromResult(gameTransaction));

            // when
            var result = await _transactionBl.CreditPlayerAsync(payoutRequest);

            // then
            Assert.IsNotNull(result);
            playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled.Should().BeTrue();
            gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled.Should().BeTrue();
            gameTransactionRepositoryGetGameTransactionPayoutAsyncHasBeenCalled.Should().BeTrue();
            gameTransactionRepositoryGameTransactionGetAsyncHasBeenCalled.Should().BeTrue();
            result.Success.Should().BeTrue();
        }

        [TestMethod]
        public async Task CreditPlayer_With_No_Player_Found_Details_Should_Return_Success_False()
        {
            // when
            PayoutRequest payoutRequest = new PayoutRequest()
            {
                GameId = _faker.Random.String2(4),
                PlayerId = _faker.Random.Int(),
                Reference = _faker.Random.String2(10)
            };

            PlayerDetail playerDetail = null!;

            bool playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = false;
            _unitOfWork.Setup(x => x.PlayerDetailRepository.GetAsync(It.IsAny<int>()))
                .Callback(() => playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = true)
                .Returns(Task.FromResult(playerDetail));

            // when
            var result = await _transactionBl.CreditPlayerAsync(payoutRequest);

            // then
            Assert.IsNotNull(result);
            playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled.Should().BeTrue();
            result.Success.Should().BeFalse();
        }

        [TestMethod]
        public async Task CreditPlayer_With_No_Initial_Bet_Should_Return_Success_False()
        {
            // when
            var payoutRequest = new PayoutRequest()
            {
                GameId = _faker.Random.String2(4),
                PlayerId = _faker.Random.Int(),
                Reference = _faker.Random.String2(10)
            };

            IEnumerable<GameTransaction> gameTransactions = new List<GameTransaction>();

            var playerDetail = new PlayerDetail()
            {
                Id = _faker.Random.Int(),
                PlayerName = _faker.Random.String2(FAKER_STRING2_LENGTH),
                Balance = 2000,
            };

            bool playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = false;
            _unitOfWork.Setup(x => x.PlayerDetailRepository.GetAsync(It.IsAny<int>()))
                .Callback(() => playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled = true)
                .Returns(Task.FromResult(playerDetail));

            bool gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled = false;
            _unitOfWork.Setup(x => x.GameTransactionRepository.GameTransactionBetsByReferenceAsync(It.IsAny<string>()))
                .Callback(() => gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled = true)
                .Returns(Task.FromResult(gameTransactions));

            // when
            var result = await _transactionBl.CreditPlayerAsync(payoutRequest);

            // then
            Assert.IsNotNull(result);
            playerDetailUnitOfWorkRepositoryGetAsyncHasBeenCalled.Should().BeTrue();
            gameTransactionRepositoryGameTransactionBetsByReferenceHasBeenCalled.Should().BeTrue();
            result.Success.Should().BeFalse();
        }
    }
}