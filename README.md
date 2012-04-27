SimpleObjectPool
================

Simple object pool for Unity3D.

Utility for using object pools that avoid the costly instantiate and destroy operations for objects with many copies and limited lifetimes, such as bullets, bombs, enemies, etc.

Started using code from here:
http://forum.unity3d.com/threads/76851-Simple-Reusable-Object-Pool-Help-limit-your-instantiations!

The .unitypackage file contains all the code and a working scene. The Assets folder is the Assets folder of the actual project. 

The sample scene demonstrates its use clearly. 

Your objects only need to know that they will have a Poolable component attached to them automatically and that they can use that component to repool themselves. 
