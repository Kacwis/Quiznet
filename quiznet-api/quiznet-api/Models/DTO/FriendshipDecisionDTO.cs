namespace quiznet_api.Models.DTO
{
    public class FriendshipDecisionDTO
    {
        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public string Decision { get; set; }
    }
}
