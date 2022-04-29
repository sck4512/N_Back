namespace MJ
{
    namespace Data
    {
        public class N_BackData
        {
            public int PlayTime => playTime;
            public int Speed => speed;
            public int N => n;


            
            private int playTime; //플레이 시간
            private int speed; //숫자 보여주는 시간
            private int n; //n_Back

            public void SetPlayTime(int _PlayTime)
            {
                playTime = _PlayTime;
            }
            public void SetSpeed(int _Speed)
            {
                speed = _Speed;
            }
            public void SetN(int _N)
            {
                n = _N;
            }
        }
    }
}
