<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="DataSimAzureApp" generation="1" functional="0" release="0" Id="c3972013-d2f9-4167-8a6a-4741ee32446e" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="DataSimAzureAppGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="DataStimulator:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/DataSimAzureApp/DataSimAzureAppGroup/LB:DataStimulator:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="DataStimulator:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/DataSimAzureApp/DataSimAzureAppGroup/MapDataStimulator:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="DataStimulatorInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/DataSimAzureApp/DataSimAzureAppGroup/MapDataStimulatorInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:DataStimulator:Endpoint1">
          <toPorts>
            <inPortMoniker name="/DataSimAzureApp/DataSimAzureAppGroup/DataStimulator/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapDataStimulator:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/DataSimAzureApp/DataSimAzureAppGroup/DataStimulator/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapDataStimulatorInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/DataSimAzureApp/DataSimAzureAppGroup/DataStimulatorInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="DataStimulator" generation="1" functional="0" release="0" software="D:\JDDataSim\DataStimulator\trunk\DataSimAzureApp\csx\Debug\roles\DataStimulator" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
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
            <sCSPolicyIDMoniker name="/DataSimAzureApp/DataSimAzureAppGroup/DataStimulatorInstances" />
            <sCSPolicyUpdateDomainMoniker name="/DataSimAzureApp/DataSimAzureAppGroup/DataStimulatorUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/DataSimAzureApp/DataSimAzureAppGroup/DataStimulatorFaultDomains" />
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
    <implementation Id="7539a5e6-866b-41e3-a688-5e00f4669f22" ref="Microsoft.RedDog.Contract\ServiceContract\DataSimAzureAppContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="8fa99b13-c244-46a8-a51f-b84cc5ad0029" ref="Microsoft.RedDog.Contract\Interface\DataStimulator:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/DataSimAzureApp/DataSimAzureAppGroup/DataStimulator:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>