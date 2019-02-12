namespace HAAGONtest
{
    internal static class Enum_sys
    {
        public enum CCDMode
        {
            创建模板,
            量块标定,
            定位参照,
            图像处理,
            调整焦距,
            定位补偿,
        }

        public enum ProdState
        {
            全部,
            OK,
            NG,
            异常,
        }

        public enum LightColor
        {
            绿色,
            红色,
            蓝色,
            灰色,
            黄色,
        }

        public enum InitError
        {
            PLC,
            Hipot,
            相机,
            机器手,
        }

        public enum DateMode
        {
            发送状态,
            发送数据,
        }

        public enum MoveMode
        {
            全程带料,
            半程空跑,
            后半程带料,
            点位运动,
            步进运动,
            调试运动,
            补偿运动,
            前半程带料,
            下料补偿,
        }
    }
}