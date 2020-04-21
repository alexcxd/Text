using DotNetTest.DataStructrue.Stack;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleCode.Test.DataStructrue
{
    /// <summary>
    /// hanoi塔
    /// </summary>
    public class HanoiTowerTest
    {
        [Test]
        public void HanoiTest()
        {
            var hanoiTower = new HanoiTower();

            for (int i = 0; i < 6; i++)
            {
                hanoiTower.stackA.Push(i.ToString());
            }

            hanoiTower.Hanoi(hanoiTower.stackA.Count, hanoiTower.stackA, hanoiTower.stackB, hanoiTower.stackC);
        }
    }
}
