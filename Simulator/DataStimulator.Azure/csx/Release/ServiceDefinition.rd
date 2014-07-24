<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="DataStimulator.Azure" generation="1" functional="0" release="0" Id="e860b581-d81c-4183-a701-902715908ecb" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="DataStimulator.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="DataStimulator:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/DataStimulator.Azure/DataStimulator.AzureGroup/LB:DataStimulator:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="DataStimulator:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/DataStimulator.Azure/DataStimulator.AzureGroup/MapDataStimulator:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="DataStimulatorInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/DataStimulator.Azure/DataStimulator.AzureGroup/MapDataStimulatorInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:DataStimulator:Endpoint1">
          <toPorts>
            <inPortMoniker name="/DataStimulator.Azure/DataStimulator.AzureGroup/DataStimulator/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapDataStimulator:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/DataStimulator.Azure/DataStimulator.AzureGroup/DataStimulator/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapDataStimulatorInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/DataStimulator.Azure/DataStimulator.AzureGroup/DataStimulatorInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="DataStimulator" generation="1" functional="0" release="0" software="D:\JDDataSim\DataStimulator\trunk\DataStimulator.Azure\csx\Release\roles\DataStimulator" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;DataStimulator&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;DataStimulator&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/DataStimulator.Azure/DataStimulator.AzureGroup/DataStimulatorInstances" />
            <sCSPolicyUpdateDomainMoniker name="/DataStimulator.Azure/DataStimulator.AzureGroup/DataStimulatorUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/DataStimulator.Azure/DataStimulator.AzureGroup/DataStimulatorFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="DataStimulatorUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="DataStimulatorFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="DataStimulatorInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="f1059302-04d6-41d8-8a32-ec1880660e93" ref="Microsoft.RedDog.Contract\ServiceContract\DataStimulator.AzureContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="6d5ff4c1-d58b-4cc2-a93b-0d98528d292e" ref="Microsoft.RedDog.Contract\Interface\DataStimulator:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/DataStimulator.Azure/DataStimulator.AzureGroup/DataStimulator:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>