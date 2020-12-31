using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Lambda.Core;

namespace ec2changeinstancetype.Services
{
    public class Ec2Services
    {        
        private static readonly IAmazonEC2 _Client = new AmazonEC2Client();
        private static string _InstanceType = Environment.GetEnvironmentVariable("instancetype");// ?? "t3a.large";
        private static string _InstanceId = Environment.GetEnvironmentVariable("instanceid");// ?? "i-sadkalsdkjsld";
        public Ec2Services()
        {
            
        }
        public static async Task ModifyInstanceAsync() {
            var describeresult = await DescribeInstanceAsync();
            string instancetype = describeresult.Reservations[0].Instances[0].InstanceType;
            _InstanceType = _InstanceType.Equals(instancetype) ? "t2.micro" : _InstanceType;

            LambdaLogger.Log($"Cambio de instancia de tipo {instancetype} a {_InstanceType}");

            await ChangeInstanceTypeAsync(_InstanceType);

            LambdaLogger.Log("Inicio de encendido de servidor.");
            await StartInstanceAsync();        
        }

        static async Task ChangeInstanceTypeAsync(string instancetype) {            
            ModifyInstanceAttributeRequest req = new ModifyInstanceAttributeRequest { 
                InstanceType = instancetype,
                InstanceId = _InstanceId
            };
            await _Client.ModifyInstanceAttributeAsync(req);   
        }
        static async Task StartInstanceAsync() {
            StartInstancesRequest req = new StartInstancesRequest { 
                InstanceIds = new List<string> { _InstanceId }                
            };
            await _Client.StartInstancesAsync(req);
        }
        static async Task<DescribeInstancesResponse> DescribeInstanceAsync() {
            DescribeInstancesRequest req = new DescribeInstancesRequest
            {
                InstanceIds = new List<string> { _InstanceId }
            };
            return await _Client.DescribeInstancesAsync(req);
        }
    }
}
