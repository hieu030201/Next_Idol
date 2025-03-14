using UnityEngine;

namespace Yun.Scripts.Utilities
{
    public static class GeneralLogic
    {
        public static Vector3[] GetWaypointsFromTwoPoints(Vector3 p1, Vector3 p2)
        {
            var rnd = new System.Random();
            var distance = Vector3.Distance(p1, p2);
            var l = distance / 3 + rnd.NextDouble() * distance / 3;
            var l2 = distance + 100;

            var vector = new Vector3(p2.x - p1.x, p2.y - p1.y);
            var c = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
            var a = (float) l / c;
            var a2 = l2 / c;

            var isLeft = rnd.Next(2);

            var p3 = new Vector3(p1.x + vector.x * a, p1.y + vector.y * a);
            var p4 = new Vector3(p1.x + vector.x * a2, p1.y + vector.y * a2);
            if (isLeft == 1)
            {
                p3.x += (float) (100 + rnd.NextDouble() * 200);
                p4.x += (float) (100 + rnd.NextDouble() * 200);
            }
            else
            {
                p3.x -= (float) (100 + rnd.NextDouble() * 50);
                p4.x -= (float) (100 + rnd.NextDouble() * 50);
            }
            
            var wayPoints = new [] {p1,p3,p2};
            return wayPoints;
        }
        
        // Hàm tính toán điểm trên đường cong Bézier bậc hai
        public static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            var u = 1 - t;
            var tt = t * t;
            var uu = u * u;

            var p = uu * p0; // (1-t)^2 * p0
            p += 2 * u * t * p1; // 2 * (1-t) * t * p1
            p += tt * p2;        // t^2 * p2

            return p;
        }

        public static Vector3[] GetWaypointsFromThreePoints(Vector3 a, Vector3 b, Vector3 c)
        {
            var midPoint1 = (a + c) / 2;
            var midPoint2 = (b + midPoint1) / 2;
            var wayPoints = new[] {a, midPoint2, c};
            return wayPoints;
        }
        
        public static float CalculateAngle(Vector3 a, Vector3 b, Vector3 c)
        {
            // Tính vector BA và BC
            var ba = a - b;
            var bc = c - b;

            // Tính tích vô hướng của BA và BC
            var dotProduct = Vector3.Dot(ba, bc);

            // Tính độ dài của BA và BC
            var magnitudeBa = ba.magnitude;
            var magnitudeBC = bc.magnitude;

            // Tính cosine của góc giữa BA và BC
            var cosTheta = dotProduct / (magnitudeBa * magnitudeBC);

            // Đảm bảo giá trị cosine nằm trong khoảng [-1, 1] để tránh lỗi do tính toán
            cosTheta = Mathf.Clamp(cosTheta, -1f, 1f);

            // Tính góc bằng radians
            var theta = Mathf.Acos(cosTheta);

            // Chuyển đổi góc từ radians sang độ
            var angleInDegrees = theta * Mathf.Rad2Deg;

            return angleInDegrees;
        }
    }
}
