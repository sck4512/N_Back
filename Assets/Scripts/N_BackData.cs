namespace MJ
{
    namespace Data
    {
        public class N_BackData
        {
            public int PlayTime => playTime;
            public int Speed => speed;
            public int N => n;


            
            private int playTime; //�÷��� �ð�
            private int speed; //���� �����ִ� �ð�
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
