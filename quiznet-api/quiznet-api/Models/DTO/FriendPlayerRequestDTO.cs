namespace quiznet_api.Models.DTO
{
    public class FriendPlayerRequestDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public bool IsActive { get; set; }  

        public int Score { get; set; }
        
        public int AvatarId { get; set; }
    }
}
