using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LORModingBase.DM
{
    /// <summary>
    /// Check datas
    /// </summary>
    class CheckDatas
    {
        /// <summary>
        /// Check datas
        /// </summary>
        public static void CheckAllDatas()
        {
            CheckCriticalPageInfos();
            CheckCardInfos();
        }

        /// <summary>
        /// Check logic for ciritical page infos
        /// </summary>
        public static void CheckCriticalPageInfos()
        {
            foreach (DS.CriticalPageInfo ciriticalInfo in MainWindow.criticalPageInfos)
            {
                if (string.IsNullOrEmpty(ciriticalInfo.bookID))
                    throw new Exception("핵심 책장 고유 ID가 입력되지 않았습니다.");
                if (string.IsNullOrEmpty(ciriticalInfo.name))
                    throw new Exception("핵심 책장 이름이 입력되지 않았습니다.");

                if (string.IsNullOrEmpty(ciriticalInfo.HP) || string.IsNullOrEmpty(ciriticalInfo.breakNum))
                    throw new Exception("핵심 책장의 HP 혹은 흐트러짐 저항이 입력되지 않았습니다.");
                if (string.IsNullOrEmpty(ciriticalInfo.minSpeedCount) || string.IsNullOrEmpty(ciriticalInfo.maxSpeedCount))
                    throw new Exception("핵심 책장의 속도 주사위 범위가 입력되지 않았습니다.");

                if (string.IsNullOrEmpty(ciriticalInfo.skinName))
                    throw new Exception("핵심 책장의 스킨 이름이 입력되지 않았습니다.");
                if (string.IsNullOrEmpty(ciriticalInfo.iconName))
                    throw new Exception("핵심 책장의 아이콘 이름이 입력되지 않았습니다.");

                if (string.IsNullOrEmpty(ciriticalInfo.description))
                    throw new Exception("핵심 책장에 대한 이야기가 입력되지 않았습니다.");

                if (!ciriticalInfo.ENEMY_IS_ENEMY_TYPE)
                {
                    if (string.IsNullOrEmpty(ciriticalInfo.chapter) || string.IsNullOrEmpty(ciriticalInfo.episode))
                        throw new Exception("핵심 책장의 에피소드가 선택되지 않았습니다.");

                    if (ciriticalInfo.dropBooks.Count <= 0)
                        throw new Exception("핵심 책장이 어느 책에서 연소되어서 나오는지가 입력되지 않았습니다.");
                }
            }
        }

        /// <summary>
        /// Check inputed card infos
        /// </summary>
        public static void CheckCardInfos()
        {
            foreach (DS.CardInfo cardInfo in MainWindow.cardInfos)
            {
                if (string.IsNullOrEmpty(cardInfo.cardID))
                    throw new Exception("전투 책장 고유 ID가 입력되지 않았습니다.");
                if (string.IsNullOrEmpty(cardInfo.name))
                    throw new Exception("전투 책장 이름이 입력되지 않았습니다.");

                if (string.IsNullOrEmpty(cardInfo.cardImage))
                    throw new Exception("전투 책장 이미지가 선택되지 않았습니다.");

                if (string.IsNullOrEmpty(cardInfo.cost))
                    throw new Exception("전투 책장 비용이 입력되지 않았습니다.");

                foreach(DS.Dice diceInfo in cardInfo.dices)
                {
                    if (string.IsNullOrEmpty(diceInfo.max))
                        throw new Exception("전투 책장 주사위의 최대값이 입력되지 않았습니다.");
                    if (string.IsNullOrEmpty(diceInfo.min))
                        throw new Exception("전투 책장 주사위의 최소값이 입력되지 않았습니다.");
                }
            }
        }
    }
}
