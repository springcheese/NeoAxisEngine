//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace Fbx {

public class Float_p : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal Float_p(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(Float_p obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~Float_p() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          FbxWrapperNativePINVOKE.delete_Float_p(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public Float_p() : this(FbxWrapperNativePINVOKE.new_Float_p(), true) {
  }

  public void assign(float value) {
    FbxWrapperNativePINVOKE.Float_p_assign(swigCPtr, value);
  }

  public float value() {
    float ret = FbxWrapperNativePINVOKE.Float_p_value(swigCPtr);
    return ret;
  }

  public SWIGTYPE_p_float cast() {
    global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.Float_p_cast(swigCPtr);
    SWIGTYPE_p_float ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_float(cPtr, false);
    return ret;
  }

  public static Float_p frompointer(SWIGTYPE_p_float t) {
    global::System.IntPtr cPtr = FbxWrapperNativePINVOKE.Float_p_frompointer(SWIGTYPE_p_float.getCPtr(t));
    Float_p ret = (cPtr == global::System.IntPtr.Zero) ? null : new Float_p(cPtr, false);
    return ret;
  }

}

}