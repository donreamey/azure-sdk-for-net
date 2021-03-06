﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
// 

using System;
using System.Linq;
using Microsoft.Azure.Test.HttpRecorder;
using Microsoft.Rest.ClientRuntime.Azure.TestFramework;
using System.Collections.Generic;
using Microsoft.Azure.Management.ApiManagement;
using Microsoft.Azure.Management.ApiManagement.Models;
using Microsoft.Azure.Management.Resources;
using Microsoft.Azure.Management.Resources.Models;
using Microsoft.Azure.Management.Storage;
using Microsoft.Azure.Management.Network;
using Xunit;
using Microsoft.Azure.Management.EventHub;

namespace ApiManagement.Tests
{
    public class ApiManagementTestBase : TestBase
    {
        private const string SubIdKey = "SubId";
        private const string ServiceNameKey = "ServiceName";
        private const string ResourceGroupNameKey = "ResourceGroup";
        private const string LocationKey = "Location";
        private const string TestCertificateKey = "TestCertificate";
        private const string TestCertificatePasswordKey = "TestCertificatePassword";
        
        public string location { get; set; }
        public string subscriptionId { get; set; }
        public ApiManagementClient client { get; set; }
        public ResourceManagementClient resourcesClient { get; set; }
        public StorageManagementClient storageClient { get; set; }
        public NetworkManagementClient networkClient { get; set; }
        public EventHubManagementClient eventHubClient { get; set; }
        public string rgName { get; internal set; }
        public Dictionary<string, string> tags { get; internal set; }
        public string serviceName { get; internal set; }
        public ApiManagementServiceResource serviceProperties { get; internal set; }
        public string base64EncodedTestCertificateData { get; internal set; }
        public string testCertificatePassword { get; internal set; }

        public ApiManagementTestBase(MockContext context)
        {
            this.client = context.GetServiceClient<ApiManagementClient>();
            this.resourcesClient = context.GetServiceClient<ResourceManagementClient>();
            this.storageClient = context.GetServiceClient<StorageManagementClient>();
            this.networkClient = context.GetServiceClient<NetworkManagementClient>();
            this.eventHubClient = context.GetServiceClient<EventHubManagementClient>();

            Initialize();
        }

        private void Initialize()
        {
            var testEnv = TestEnvironmentFactory.GetTestEnvironment();

            if (HttpMockServer.Mode == HttpRecorderMode.Record)
            {
                this.serviceName = testEnv.ConnectionString.KeyValuePairs[ServiceNameKey];
                if (string.IsNullOrEmpty(this.serviceName))
                {
                    this.serviceName = TestUtilities.GenerateName("sdktestapim");
                }

                this.location = testEnv.ConnectionString.KeyValuePairs[LocationKey];
                if (string.IsNullOrEmpty(this.location))
                {
                    this.location = GetLocation();
                }

                this.rgName = testEnv.ConnectionString.KeyValuePairs[ResourceGroupNameKey];
                if (string.IsNullOrEmpty(this.rgName))
                {
                    rgName = TestUtilities.GenerateName("sdktestrg");
                    resourcesClient.ResourceGroups.CreateOrUpdate(rgName, new ResourceGroup { Location = this.location });
                }
                                
                this.base64EncodedTestCertificateData = testEnv.ConnectionString.KeyValuePairs[TestCertificateKey];
                if (!string.IsNullOrEmpty(this.base64EncodedTestCertificateData))
                {
                    HttpMockServer.Variables[TestCertificateKey] = base64EncodedTestCertificateData;
                }
                this.testCertificatePassword = testEnv.ConnectionString.KeyValuePairs[TestCertificatePasswordKey];
                if(!string.IsNullOrEmpty(this.testCertificatePassword))
                {
                    HttpMockServer.Variables[TestCertificatePasswordKey] = testCertificatePassword;
                }
                
                this.subscriptionId = testEnv.SubscriptionId;
                HttpMockServer.Variables[SubIdKey] = subscriptionId;                
                HttpMockServer.Variables[ServiceNameKey] = this.serviceName;
                HttpMockServer.Variables[LocationKey] = this.location;
                HttpMockServer.Variables[ResourceGroupNameKey] = this.rgName;
            }
            else if (HttpMockServer.Mode == HttpRecorderMode.Playback)
            {
                this.subscriptionId = testEnv.SubscriptionId;
                subscriptionId = HttpMockServer.Variables[SubIdKey];
                rgName = HttpMockServer.Variables[ResourceGroupNameKey];
                serviceName = HttpMockServer.Variables[ServiceNameKey];
                location = HttpMockServer.Variables[LocationKey];
                HttpMockServer.Variables.TryGetValue(TestCertificateKey, out var testcertificate); 
                if(!string.IsNullOrEmpty(testcertificate))
                {
                    this.base64EncodedTestCertificateData = testcertificate;
                }
                HttpMockServer.Variables.TryGetValue(TestCertificatePasswordKey, out var testCertificatePwd);
                if(!string.IsNullOrEmpty(testCertificatePwd))
                {
                    this.testCertificatePassword = testCertificatePwd;
                }
            }            

            tags = new Dictionary<string, string> { { "tag1", "value1" }, { "tag2", "value2" }, { "tag3", "value3" } };

            serviceProperties = new ApiManagementServiceResource
            {
                Sku = new ApiManagementServiceSkuProperties
                {
                    Name = SkuType.Developer,
                    Capacity = 1
                },
                Location = location,
                PublisherEmail = "apim@autorestsdk.com",
                PublisherName = "autorestsdk",
                Tags = tags
            };
        }

        public void TryCreateApiManagementService()
        {
            this.client.ApiManagementService.CreateOrUpdate(
                resourceGroupName: this.rgName,
                serviceName: this.serviceName,
                parameters: this.serviceProperties);

            var service = this.client.ApiManagementService.Get(this.rgName, this.serviceName);
            Assert.Equal(this.serviceName, service.Name);
        }

        public string GetLocation(string regionIn = "US")
        {
            var provider = this.resourcesClient.Providers.Get("Microsoft.ApiManagement");
            return provider.ResourceTypes.Where(
                (resType) =>
                {
                    if (resType.ResourceType == "service")
                        return true;
                    else
                        return false;
                }
                ).First().Locations.Where(l => l.Contains(regionIn)).FirstOrDefault();
        }
    }
}