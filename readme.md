# be(e) develop CrmSvcUtil.Extensions

[![Build status](https://ci.appveyor.com/api/projects/status/h08kj4jw0ogp8gex?svg=true)](https://ci.appveyor.com/project/JJauss/xrm-tooling)


## Intention
The CrmSvcUtil.exe tool which is provided by the Dynamics CRM SDK, allows to customize the process of the code generation. Out of the box, the code generation has some impediments. 

+ No capability to filter generated entities and attributes
+ Generated code is stressed with the publisher prefix (including undescores)
+ When customizing is not compatible with C# code some magic rules are applied to create names. No manual mapping is possible
+ Enumerations are not gnerated for OptionSets
+ Not possible to inject custom base classes for Entity.
+ Code generation source cannot be persistet (cached) and reused during build
