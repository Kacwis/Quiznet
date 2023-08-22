using System.Linq.Expressions;
using System.Security.Claims;
using quiznet_api.Handlers.IHandlers;
using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository.IRepository;
using quiznet_api.Services.IServices;

[TestFixture]
public class UserServiceTests
{
    private UserService _userService;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IPlayerRepository> _playerRepositoryMock;
    private Mock<IFriendshipRepository> _friendshipRepositoryMock;
    private Mock<IBlockedPlayerRepository> _blockedPlayerRepositoryMock;
    private Mock<IMessageService> _messageServiceMock;
    private Mock<IGameService> _gameServiceMock;
    private Mock<IJwtHandler> _jwtHandlerMock;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _playerRepositoryMock = new Mock<IPlayerRepository>();
        _friendshipRepositoryMock = new Mock<IFriendshipRepository>();
        _blockedPlayerRepositoryMock = new Mock<IBlockedPlayerRepository>();
        _messageServiceMock = new Mock<IMessageService>();
        _gameServiceMock = new Mock<IGameService>();
        _jwtHandlerMock = new Mock<IJwtHandler>();

        _userService = new UserService(
            _userRepositoryMock.Object,
            _playerRepositoryMock.Object,
            _jwtHandlerMock.Object,
            _friendshipRepositoryMock.Object,
            _messageServiceMock.Object,
            _gameServiceMock.Object,
            _blockedPlayerRepositoryMock.Object
        );
    }
    
    [Test]
    public async Task RegisterUser_SuccessfulRegistration_ReturnsRegisteredUserAndCreatesPlayer()
    {
        var username = "User";
        var password = "password";
        var registeredUserId = 1;
        var registrationRequest = new RegistrationRequestDTO
        {
            Username = username,
            Password = password
        };

        var registeredUser = new User
        {
            Id = registeredUserId,
            Username = username,
            Password = password,
        };

        _userRepositoryMock.Setup(repo => repo.Register(It.IsAny<RegistrationRequestDTO>()))
            .ReturnsAsync(registeredUser);

        
        var result = await _userService.RegisterUser(registrationRequest);

        Assert.That(result.Username, Is.EqualTo(registrationRequest.Username));
        Assert.That(result.Password, Is.EqualTo(registrationRequest.Password));
        
        
        _playerRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<Player>(p =>
            p.User == registeredUser &&
            p.Score == 0 &&
            p.LastOnline.Date == DateTime.Now.Date
        )), Times.Once);

        _playerRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
    }
    
    [Test]
    public async Task BlockFriend_FriendIsBlocked_SuccessfullyBlocksFriend()
    {
        var friendId = 2;
        var senderId = 1;
        
        var senderPlayer = new Player
        {
            Id = senderId,
            FriendshipsIncoming = new List<Friendship>(){new (){Id = 1, SenderId = friendId, ReceiverId = senderId}},
            MessagesIncoming = new List<Message>(),
            MessagesOutgoing = new List<Message>()
            
        };

        var friendPlayer = new Player
        {
            Id = friendId,
            FriendshipsOutgoing = new List<Friendship>(){new (){Id = 1, SenderId = friendId, ReceiverId = senderId}},
            MessagesIncoming = new List<Message>(),
            MessagesOutgoing = new List<Message>()
        };

        _playerRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Player, bool>>>(), true))
            .ReturnsAsync(friendPlayer);

        
        await _userService.BlockFriend(senderPlayer, friendId);
        
        _blockedPlayerRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<BlockedPlayer>(blocked =>
            blocked.BlockedAt.Date == DateTime.Now.Date &&
            blocked.BlockingPlayerId == senderPlayer.Id &&
            blocked.PlayerBlockedId == friendPlayer.Id
        )), Times.Once);
    }
    
    [Test]
    public async Task RemoveFriendship_FriendshipExists_SuccessfullyRemovesFriendshipAndMessages()
    {
        var senderId = 1;
        var friendId = 2;
        var senderPlayer = new Player
        {
            Id = senderId,
            FriendshipsOutgoing = new List<Friendship>
            {
                new Friendship
                {
                    ReceiverId = friendId,
                    SenderId = senderId
                }
            },
            FriendshipsIncoming = new List<Friendship>(),
            MessagesOutgoing = new List<Message>
            {
                new Message
                {
                    ReceiverId = friendId,
                    SenderId = senderId
                }
            },
            MessagesIncoming = new List<Message>()
        };

        

        var friendPlayer = new Player
        {
            Id = friendId,
            FriendshipsIncoming = new List<Friendship>()
            {
                new Friendship()
                {
                    SenderId = senderId,
                    ReceiverId = friendId
                }
            },
            FriendshipsOutgoing = new List<Friendship>(),
            MessagesIncoming = new List<Message>
            {
                new Message
                {
                    SenderId = senderId,
                    ReceiverId = friendId
                }
            },
            MessagesOutgoing = new List<Message>()
            
        };

        var friendship = senderPlayer.FriendshipsOutgoing.FirstOrDefault();

        _playerRepositoryMock
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Player, bool>>>(), true))
            .ReturnsAsync(friendPlayer);
        _friendshipRepositoryMock
            .Setup(repo => repo.RemoveAsync(friendship))
            .Returns(Task.CompletedTask);

        var capturedMessages = new List<Message>();
        
        _messageServiceMock
            .Setup(service => service.RemoveMessageAsync(It.IsAny<Message>()))
            .Callback<Message>(message => capturedMessages.Add(message))             
            .Returns(Task.CompletedTask);

        // Act
        await _userService.RemoveFriendship(senderPlayer, friendId);
        
        _friendshipRepositoryMock.Verify(repo => repo.RemoveAsync(friendship), Times.Once);

        var expectedMsgNumber = 1;
        
        Assert.That(capturedMessages.Count, Is.EqualTo(expectedMsgNumber));

        foreach (var capturedMessage in capturedMessages)
        {
            Assert.That(capturedMessage.SenderId == senderPlayer.Id ||
                        capturedMessage.ReceiverId == senderPlayer.Id, Is.True);
            
            Assert.That(capturedMessage.SenderId == friendPlayer.Id ||
                        capturedMessage.ReceiverId == friendPlayer.Id, Is.True);
        }
    }

    
    
    
    
    
}