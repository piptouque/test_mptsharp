
### MPTSharp

#### C# bindings for libopenmpt

##### Overview

I went for object-oriented bindings, following the original C++ version of the
library.

Now with partial support of module, module_ext, and the interactive extension.


| C               | C++              | C#             |
| :-------------: | :----------:     | -----------:   |
|  openmpt_module | openmpt::module   | OpenMpt.Module |
|  openmpt_module_ext  | openmpt::module_ext | OpenMpt.ModuleExt  |
|  openmpt_module_ext_interface_interactive   | openmpt::ext::interactive  | OpenMpt.Ext.Interactive |


As is shown here, I tried to match C++ namespaces and class names, however I
decided to use C# naming conversions with Pascal-case for classes and
namespaces.

##### How to use

Will need to load the original libopenmpt dll, probably.
In Unity 2019.3 I only needed to import the library from the Inspector.
