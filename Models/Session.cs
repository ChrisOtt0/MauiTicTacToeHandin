using System.Runtime.CompilerServices;

namespace ServerTicTacToeHandin.Models
{
    public class Session
    {
        #region Public Properties
        public int Id { get; set; }
        public Player[] Players { get; set; }
        public List<List<Field>> Board { get; set; } = new List<List<Field>>();
        public Status Status { get; set; } = Status.On;
        public DateTime? Ended { get; set; } = null;
        public Field Winner { get; set; } = Field.Empty;
        #endregion

        
    }
}
