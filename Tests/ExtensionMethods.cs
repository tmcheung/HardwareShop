﻿using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    public static class ExtensionMethods
    {
        public static void SetupIQueryable<T>(this Mock<T> mock, IQueryable queryable)
            where T : class, IQueryable
        {
            mock.Setup(r => r.GetEnumerator()).Returns(queryable.GetEnumerator());
            mock.SetupGet(r => r.Provider).Returns(queryable.Provider);
            mock.SetupGet(r => r.ElementType).Returns(queryable.ElementType);
            mock.SetupGet(r => r.Expression).Returns(queryable.Expression);
        }
    }
}
