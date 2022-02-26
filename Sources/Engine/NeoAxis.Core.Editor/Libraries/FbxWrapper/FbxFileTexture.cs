//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace Internal.Fbx {

public class FbxFileTexture : FbxTexture {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;

  internal FbxFileTexture(global::System.IntPtr cPtr, bool cMemoryOwn) : base(FbxWrapperNativePINVOKE.FbxFileTexture_SWIGUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(FbxFileTexture obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          throw new global::System.MethodAccessException("C++ destructor does not have public access");
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public static FbxClassId ClassId {
    set {
      FbxWrapperNativePINVOKE.FbxFileTexture_ClassId_set(FbxClassId.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxFileTexture_ClassId_get();
      FbxClassId ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxClassId(cPtr, false);
      return ret;
    } 
  }

  public override FbxClassId GetClassId() {
    FbxClassId ret = new FbxClassId(FbxWrapperNativePINVOKE.FbxFileTexture_GetClassId(swigCPtr), true);
    return ret;
  }

  public new static FbxFileTexture Create(FbxManager pManager, string pName) {
    global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxFileTexture_Create__SWIG_0(FbxManager.getCPtr(pManager), pName);
    FbxFileTexture ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxFileTexture(cPtr, false);
    return ret;
  }

  public new static FbxFileTexture Create(FbxObject pContainer, string pName) {
    global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxFileTexture_Create__SWIG_1(FbxObject.getCPtr(pContainer), pName);
    FbxFileTexture ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxFileTexture(cPtr, false);
    return ret;
  }

  public SWIGTYPE_p_FbxPropertyTT_bool_t UseMaterial {
    set {
      FbxWrapperNativePINVOKE.FbxFileTexture_UseMaterial_set(swigCPtr, SWIGTYPE_p_FbxPropertyTT_bool_t.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxFileTexture_UseMaterial_get(swigCPtr);
      SWIGTYPE_p_FbxPropertyTT_bool_t ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_FbxPropertyTT_bool_t(cPtr, false);
      return ret;
    } 
  }

  public SWIGTYPE_p_FbxPropertyTT_bool_t UseMipMap {
    set {
      FbxWrapperNativePINVOKE.FbxFileTexture_UseMipMap_set(swigCPtr, SWIGTYPE_p_FbxPropertyTT_bool_t.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxFileTexture_UseMipMap_get(swigCPtr);
      SWIGTYPE_p_FbxPropertyTT_bool_t ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_FbxPropertyTT_bool_t(cPtr, false);
      return ret;
    } 
  }

  public override void Reset() {
    FbxWrapperNativePINVOKE.FbxFileTexture_Reset(swigCPtr);
  }

  public bool SetFileName(string pName) {
    bool ret = FbxWrapperNativePINVOKE.FbxFileTexture_SetFileName(swigCPtr, pName);
    return ret;
  }

  public bool SetRelativeFileName(string pName) {
    bool ret = FbxWrapperNativePINVOKE.FbxFileTexture_SetRelativeFileName(swigCPtr, pName);
    return ret;
  }

  public string GetFileName() {
    string ret = FbxWrapperNativePINVOKE.FbxFileTexture_GetFileName(swigCPtr);
    return ret;
  }

  public string GetRelativeFileName() {
    string ret = FbxWrapperNativePINVOKE.FbxFileTexture_GetRelativeFileName(swigCPtr);
    return ret;
  }

  public void SetMaterialUse(FbxFileTexture.EMaterialUse pMaterialUse) {
    FbxWrapperNativePINVOKE.FbxFileTexture_SetMaterialUse(swigCPtr, (int)pMaterialUse);
  }

  public FbxFileTexture.EMaterialUse GetMaterialUse() {
    FbxFileTexture.EMaterialUse ret = (FbxFileTexture.EMaterialUse)FbxWrapperNativePINVOKE.FbxFileTexture_GetMaterialUse(swigCPtr);
    return ret;
  }

  public override FbxObject Copy(FbxObject pObject) {
    FbxObject ret = new FbxObject(FbxWrapperNativePINVOKE.FbxFileTexture_Copy(swigCPtr, FbxObject.getCPtr(pObject)), false);
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public bool eq(FbxFileTexture pTexture) {
    bool ret = FbxWrapperNativePINVOKE.FbxFileTexture_eq(swigCPtr, FbxFileTexture.getCPtr(pTexture));
    if (FbxWrapperNativePINVOKE.SWIGPendingException.Pending) throw FbxWrapperNativePINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public FbxString GetMediaName() {
    FbxString ret = new FbxString(FbxWrapperNativePINVOKE.FbxFileTexture_GetMediaName(swigCPtr), false);
    return ret;
  }

  public void SetMediaName(string pMediaName) {
    FbxWrapperNativePINVOKE.FbxFileTexture_SetMediaName(swigCPtr, pMediaName);
  }

  public static FbxFileTexture Cast(FbxObject arg0) {
    global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.FbxFileTexture_Cast(FbxObject.getCPtr(arg0));
    FbxFileTexture ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxFileTexture(cPtr, false);
    return ret;
  }

  public enum EMaterialUse {
    eModelMaterial,
    eDefaultMaterial
  }

}

}