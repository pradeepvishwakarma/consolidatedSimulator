<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="SimulationSocket.Azure" generation="1" functional="0" release="0" Id="8cb54ba4-02c2-4aac-bfe6-802ef8d5cbc4" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="SimulationSocket.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="SimulationSocket:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/SimulationSocket.Azure/SimulationSocket.AzureGroup/LB:SimulationSocket:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="SimulationSocket:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SimulationSocket.Azure/SimulationSocket.AzureGroup/MapSimulationSocket:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="SimulationSocketInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SimulationSocket.Azure/SimulationSocket.AzureGroup/MapSimulationSocketInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:SimulationSocket:Endpoint1">
          <toPorts>
            <inPortMoniker name="/SimulationSocket.Azure/SimulationSocket.AzureGroup/SimulationSocket/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapSimulationSocket:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SimulationSocket.Azure/SimulationSocket.AzureGroup/SimulationSocket/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapSimulationSocketInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SimulationSocket.Azure/SimulationSocket.AzureGroup/SimulationSocketInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="SimulationSocket" generation="1" functional="0" release="0" software="C:\Users\Nilesh\Desktop\Harvest Simulator\Simulator\SimulationSocket.Azure\csx\Release\roles\SimulationSocket" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;SimulationSocket&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;SimulationSocket&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SimulationSocket.Azure/SimulationSocket.AzureGroup/SimulationSocketInstances" />
            <sCSPolicyUpdateDomainMoniker name="/SimulationSocket.Azure/SimulationSocket.AzureGroup/SimulationSocketUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/SimulationSocket.Azure/SimulationSocket.AzureGroup/SimulationSocketFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="SimulationSocketUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="SimulationSocketFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="SimulationSocketInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="479be64f-4571-4d39-b7a3-6211e6edfca4" ref="Microsoft.RedDog.Contract\ServiceContract\SimulationSocket.AzureContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="7e0360b8-6267-49f9-9eb7-ca3d29d4e43e" ref="Microsoft.RedDog.Contract\Interface\SimulationSocket:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/SimulationSocket.Azure/SimulationSocket.AzureGroup/SimulationSocket:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>