/* 
 * My API
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using RestSharp;
using NUnit.Framework;

using IO.Swagger.Client;
using IO.Swagger.Api;
using IO.Swagger.Model;

namespace IO.Swagger.Test
{
    /// <summary>
    ///  Class for testing WeekApi
    /// </summary>
    /// <remarks>
    /// This file is automatically generated by Swagger Codegen.
    /// Please update the test case below to test the API endpoint.
    /// </remarks>
    [TestFixture]
    public class WeekApiTests
    {
        private WeekApi instance;

        /// <summary>
        /// Setup before each unit test
        /// </summary>
        [SetUp]
        public void Init()
        {
            instance = new WeekApi();
        }

        /// <summary>
        /// Clean up after each unit test
        /// </summary>
        [TearDown]
        public void Cleanup()
        {

        }

        /// <summary>
        /// Test an instance of WeekApi
        /// </summary>
        [Test]
        public void InstanceTest()
        {
            // TODO uncomment below to test 'IsInstanceOfType' WeekApi
            //Assert.IsInstanceOfType(typeof(WeekApi), instance, "instance is a WeekApi");
        }

        
        /// <summary>
        /// Test V1WeekByIdDelete
        /// </summary>
        [Test]
        public void V1WeekByIdDeleteTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //long? id = null;
            //var response = instance.V1WeekByIdDelete(id);
            //Assert.IsInstanceOf<ResponseIEnumerableWeekDTO> (response, "response is ResponseIEnumerableWeekDTO");
        }
        
        /// <summary>
        /// Test V1WeekByIdGet
        /// </summary>
        [Test]
        public void V1WeekByIdGetTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //long? id = null;
            //var response = instance.V1WeekByIdGet(id);
            //Assert.IsInstanceOf<ResponseWeekDTO> (response, "response is ResponseWeekDTO");
        }
        
        /// <summary>
        /// Test V1WeekByIdPut
        /// </summary>
        [Test]
        public void V1WeekByIdPutTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //long? id = null;
            //WeekDTO newWeek = null;
            //var response = instance.V1WeekByIdPut(id, newWeek);
            //Assert.IsInstanceOf<ResponseWeekDTO> (response, "response is ResponseWeekDTO");
        }
        
        /// <summary>
        /// Test V1WeekGet
        /// </summary>
        [Test]
        public void V1WeekGetTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //var response = instance.V1WeekGet();
            //Assert.IsInstanceOf<ResponseIEnumerableWeekDTO> (response, "response is ResponseIEnumerableWeekDTO");
        }
        
        /// <summary>
        /// Test V1WeekPost
        /// </summary>
        [Test]
        public void V1WeekPostTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //WeekDTO newWeek = null;
            //var response = instance.V1WeekPost(newWeek);
            //Assert.IsInstanceOf<ResponseWeekDTO> (response, "response is ResponseWeekDTO");
        }
        
    }

}
