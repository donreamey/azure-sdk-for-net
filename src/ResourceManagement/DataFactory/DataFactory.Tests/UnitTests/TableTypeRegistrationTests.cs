﻿// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

using System;
using System.Globalization;
using System.Reflection;
using DataFactory.Tests.Framework;
using DataFactory.Tests.UnitTests.TestClasses;
using Microsoft.Azure.Management.DataFactories;
using Microsoft.Azure.Management.DataFactories.Models;
using Xunit;

namespace DataFactory.Tests.UnitTests
{
    public class TableTypeRegistrationTests : UnitTestBase
    {
        private TableOperations Operations
        {
            get
            {
                return (TableOperations)this.Client.Tables;
            }
        }

        [Fact]
        [Trait(TraitName.TestType, TestType.Unit)]
        [Trait(TraitName.Function, TestType.Registration)]
        public void CanRegisterTableType()
        {
            this.Client.RegisterType<MyTableType>(true);

            Assert.True(
                this.Client.TypeIsRegistered<MyTableType>(),
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Type '{0}' was not successfully registered.",
                    typeof(MyTableType).Name));
        }

        [Fact]
        [Trait(TraitName.TestType, TestType.Unit)]
        [Trait(TraitName.Function, TestType.Registration)]
        public void RegisteringTableTypeWithReservedNameThrowsException()
        {
            InvalidOperationException ex =
                Assert.Throws<InvalidOperationException>(
                    () => this.Client.RegisterType<AzureSqlTableDataset>());

            Assert.True(ex.Message.Contains("cannot be locally registered because it has the same name"));
        }

        [Fact]
        [Trait(TraitName.TestType, TestType.Unit)]
        [Trait(TraitName.Function, TestType.Registration)]
        public void RegisteringTableTypeTwiceWithoutForceThrowsException()
        {
            this.Client.RegisterType<MyTableType>(true);

            InvalidOperationException ex =
                Assert.Throws<InvalidOperationException>(() => this.Client.RegisterType<MyTableType>());
            Assert.True(ex.Message.Contains("is already registered"));
        }

        [Fact]
        [Trait(TraitName.TestType, TestType.Unit)]
        [Trait(TraitName.Function, TestType.Registration)]
        public void CanGetRegisteredActivityCaseInsensitive()
        {
            AdfTypeNameAttribute att = typeof(AzureBlobDataset).GetCustomAttribute<AdfTypeNameAttribute>(true);
            Assert.NotNull(att);

            // Get the type named used for de/ser
            string typeName = att.TypeName;
            Assert.NotNull(typeName);
            Assert.NotEmpty(typeName);

            // Ensure that the type name is not already all lowercase
            string typeNameLower = typeName.ToLowerInvariant();
            Assert.NotEqual(typeName, typeNameLower, StringComparer.Ordinal);

            Type type;
            Assert.True(this.Operations.Converter.TryGetRegisteredType(typeNameLower, out type));
            Assert.Equal(typeof(AzureBlobDataset), type);
        }
    }
}
