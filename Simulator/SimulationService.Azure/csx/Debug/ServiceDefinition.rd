<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="SimulationService.Azure" generation="1" functional="0" release="0" Id="66d988a1-cd22-41a2-8509-539864c206eb" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="SimulationService.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="SimulationService:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/SimulationService.Azure/SimulationService.AzureGroup/LB:SimulationService:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="SimulationService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SimulationService.Azure/SimulationService.AzureGroup/MapSimulationService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="SimulationServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SimulationService.Azure/SimulationService.AzureGroup/MapSimulationServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:SimulationService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/SimulationService.Azure/SimulationService.AzureGroup/SimulationService/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapSimulationService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SimulationService.Azure/SimulationService.AzureGroup/SimulationService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapSimulationServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SimulationService.Azure/SimulationService.AzureGroup/SimulationServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="SimulationService" generation="1" functional="0" release="0" software="D:\JDDataSim\DataStimulator\trunk\SimulationService.Azure\csx\Debug\roles\SimulationService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;SimulationService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;SimulationService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SimulationService.Azure/SimulationService.AzureGroup/SimulationServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/SimulationService.Azure/SimulationService.AzureGroup/SimulationServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/SimulationService.Azure/SimulationService.AzureGroup/SimulationServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="SimulationServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="SimulationServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="SimulationServiceInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="176d0004-c978-42e4-bebb-058cbb7431f4" ref="Microsoft.RedDog.Contract\ServiceContract\SimulationService.AzureContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="cc9acb16-3251-47ca-b58d-c948afeb4f5e" ref="Microsoft.RedDog.Contract\Interface\SimulationService:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/SimulationService.Azure/SimulationService.AzureGroup/SimulationService:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>