﻿//
// Copyright 2017 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;


namespace Steeltoe.CircuitBreaker.Hystrix.Test
{
    public class TestCircuitBreaker : IHystrixCircuitBreaker
    {

        public readonly HystrixCommandMetrics metrics;
        private bool forceShortCircuit = false;

        public TestCircuitBreaker()
        {
            this.metrics = HystrixCircuitBreakerTest.GetMetrics(HystrixCommandOptionsTest.GetUnitTestOptions());
            forceShortCircuit = false;
        }

        public TestCircuitBreaker(IHystrixCommandKey commandKey)
        {
            this.metrics = HystrixCircuitBreakerTest.GetMetrics(commandKey, HystrixCommandOptionsTest.GetUnitTestOptions());
            forceShortCircuit = false;
        }

        public TestCircuitBreaker SetForceShortCircuit(bool value)
        {
            this.forceShortCircuit = value;
            return this;
        }


        public bool IsOpen
        {
            get
            {
                //output.WriteLine("metrics : " + metrics.CommandKey.Name + " : " + metrics.Healthcounts);
                if (forceShortCircuit)
                {
                    return true;
                }
                else
                {
                    return metrics.Healthcounts.ErrorCount >= 3;
                }
            }
        }


        public void MarkSuccess()
        {
            // we don't need to do anything since we're going to permanently trip the circuit
        }


        public bool AllowRequest
        {
            get { return !IsOpen; }
        }

    }
}
