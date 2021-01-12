using System.Text.RegularExpressions;

namespace LORModingBase.Tools
{
    /// <summary>
    /// 값이 알맞은지 검증하기위한 함수들이 있습니다.
    /// </summary>
    class CheckInput
    {
        /// <summary>
        /// 정수가 제대로 입력되었는지 검증합니다.
        /// </summary>
        public static bool IsIntegerInputed(string textToTest)
        {
            if (!string.IsNullOrEmpty(textToTest) && !Regex.IsMatch(textToTest, "^[0-9]+$"))
                return false;
            else
                return true;
        }

        /// <summary>
        /// 소수가 제대로 입력되었는지 검증합니다.
        /// </summary>
        public static bool IsDoubleInputed(string textToTest)
        {
            if (!string.IsNullOrEmpty(textToTest) && !Regex.IsMatch(textToTest, "^([0-9]+|[0-9]+\\.[0-9]+)$"))
                return false;
            else
                return true;
        }
    }
}
