namespace Factory
{
    public class Paths
    {
        private BuildSystem BuildSystem;

        public Paths(BuildSystem buildSystem)
        {
            BuildSystem = buildSystem;
        }
        
        public void Path1()
        {
            int width = 8;
            int height = 8;

            for (int y = 0; y < height; y++)
            {
                if (y % 2 == 0)
                {
                    // слева направо
                    for (int x = 0; x < width; x++)
                    {
                        BeltDirections inDir;
                        BeltDirections outDir;

                        if (x == 0)
                            inDir = (y == 0) ? BeltDirections.Up : BeltDirections.Down;
                        else
                            inDir = BeltDirections.Right;

                        if (x == width - 1)
                            outDir = (y < height - 1) ? BeltDirections.Up : BeltDirections.Left;
                        else
                            outDir = BeltDirections.Right;

                        BuildSystem.CreateBelt(x, y, inDir, outDir);
                    }
                }
                else
                {
                    // справа налево
                    for (int x = width - 1; x >= 0; x--)
                    {
                        BeltDirections inDir;
                        BeltDirections outDir;

                        if (x == width - 1)
                            inDir = BeltDirections.Up;
                        else
                            inDir = BeltDirections.Left;

                        if (x == 0)
                            outDir = (y < height - 1) ? BeltDirections.Up : BeltDirections.Right;
                        else
                            outDir = BeltDirections.Left;

                        BuildSystem.CreateBelt(x, y, inDir, outDir);
                    }
                }
            }
        }

        public void Path2()
        {
            for (int i = 0; i < 5; i++)
                BuildSystem.CreateBelt(0, i, BeltDirections.Up, BeltDirections.Up);
            BuildSystem.CreateBelt(0, 5, BeltDirections.Up, BeltDirections.Right);
            for (int i = 1; i < 5; i++)
                BuildSystem.CreateBelt(i, 5, BeltDirections.Right, BeltDirections.Right);
            BuildSystem.CreateBelt(5, 5, BeltDirections.Right, BeltDirections.Down);
        }

        public void Path3()
        {
            BuildSystem.CreateBelt(0, 0, BeltDirections.Up, BeltDirections.Up);
            BuildSystem.CreateBelt(0, 1, BeltDirections.Up, BeltDirections.Right);
            BuildSystem.CreateBelt(1, 1, BeltDirections.Right, BeltDirections.Right);
            BuildSystem.CreateBelt(2, 1, BeltDirections.Right, BeltDirections.Down);
            BuildSystem.CreateBelt(2, 0, BeltDirections.Down, BeltDirections.Down);
            BuildSystem.CreateBelt(2, -1, BeltDirections.Down, BeltDirections.Left);
            BuildSystem.CreateBelt(1, -1, BeltDirections.Left, BeltDirections.Left);
            BuildSystem.CreateBelt(0, -1, BeltDirections.Left, BeltDirections.Up);
        }

        public void Path4()
        {
            BuildSystem.CreateBeltQueue(new int[]
            {
                // y = 0 →
                0,0, 1,0, 2,0, 3,0, 4,0, 5,0, 6,0, 7,0,

                // вверх
                7,1,

                // y = 1 ←
                6,1, 5,1, 4,1, 3,1, 2,1, 1,1, 0,1,

                // вверх
                0,2,

                // y = 2 →
                1,2, 2,2, 3,2, 4,2, 5,2, 6,2, 7,2,

                // вверх
                7,3,

                // y = 3 ←
                6,3, 5,3, 4,3, 3,3, 2,3, 1,3, 0,3,

                // вверх
                0,4,

                // y = 4 →
                1,4, 2,4, 3,4, 4,4, 5,4, 6,4, 7,4,

                // вверх
                7,5,

                // y = 5 ←
                6,5, 5,5, 4,5, 3,5, 2,5, 1,5, 0,5,

                // вверх
                0,6,

                // y = 6 →
                1,6, 2,6, 3,6, 4,6, 5,6, 6,6, 7,6,

                // вверх
                7,7,

                // y = 7 ← (финал)
                6,7, 5,7, 4,7, 3,7, 2,7, 1,7, 0,7
            });
        }

        public void Path5()
        {
            BuildSystem.CreateBeltQueue(new []{0,0, 0,1, 1,1, 2,1, 2,0, 2,-1, 1,-1, 0,-1} , BeltDirections.Up, BeltDirections.Up);
        }

        public void Path6()
        {
            BuildSystem.CreateBeltQueue(new []{0,0, 0,1, 0,2} , BeltDirections.Up, BeltDirections.Up);
        }
        public void Path7()
        {
            BuildSystem.CreateBeltQueue(new []{0,0, -1,0, -1,-1, 0,-1} , BeltDirections.Up, BeltDirections.Up);
        }
    }
}