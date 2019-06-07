﻿using System.Collections.Generic;

namespace Eagle
{
    public class ResultTestSuite 
    {
        public ResultTestSuite()
        {
            TestSuites = new List<ResultTestSuite>();
            TestCases = new List<ResultTestCase>();
        }

        public List<ResultTestSuite> TestSuites { get; set; }

        public List<ResultTestCase> TestCases { get; set; }

        public string FullName { get; set; }

    }
}