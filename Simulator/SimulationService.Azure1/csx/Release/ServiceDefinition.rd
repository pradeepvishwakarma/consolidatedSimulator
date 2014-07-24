﻿<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="SimulationService.Azure1" generation="1" functional="0" release="0" Id="08b823e2-2d57-4265-9588-b7238919b9c2" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="SimulationService.Azure1Group" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="SimulationService:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/SimulationService.Azure1/SimulationService.Azure1Group/LB:SimulationService:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="SimulationService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/SimulationService.Azure1/SimulationService.Azure1Group/MapSimulationService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="SimulationServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SimulationService.Azure1/SimulationService.Azure1Group/MapSimulationServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:SimulationService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/SimulationService.Azure1/SimulationService.Azure1Group/SimulationService/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapSimulationService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/SimulationService.Azure1/SimulationService.Azure1Group/SimulationService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapSimulationServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SimulationService.Azure1/SimulationService.Azure1Group/SimulationServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="SimulationService" generation="1" functional="0" release="0" software="D:\JDDataSim\DataStimulator\trunk\SimulationService.Azure1\csx\Release\roles\SimulationService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
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
            <sCSPolicyIDMoniker name="/SimulationService.Azure1/SimulationService.Azure1Group/SimulationServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/SimulationService.Azure1/SimulationService.Azure1Group/SimulationServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/SimulationService.Azure1/SimulationService.Azure1Group/SimulationServiceFaultDomains" />
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
    <implementation Id="40d31bb4-c6a6-4636-93ea-17edfb0727bc" ref="Microsoft.RedDog.Contract\ServiceContract\SimulationService.Azure1Contract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="41cfc41e-d8cd-4423-9bac-284d4a8232fe" ref="Microsoft.RedDog.Contract\Interface\SimulationService:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/SimulationService.Azure1/SimulationService.Azure1Group/SimulationService:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>