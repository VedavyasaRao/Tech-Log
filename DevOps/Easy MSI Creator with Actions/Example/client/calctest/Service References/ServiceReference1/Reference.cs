//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.19132
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace calctest.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://tempuri.org/DE10BAE6-FC43-3BA6-A05B-6D652FB2ACE7", ConfigurationName="ServiceReference1.ISillyCalc_WSC_1", SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface ISillyCalc_WSC_1 {
        
        // CODEGEN: Parameter 'addResult' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/DE10BAE6-FC43-3BA6-A05B-6D652FB2ACE7/ISillyCalc_WSC_1/add", ReplyAction="http://tempuri.org/DE10BAE6-FC43-3BA6-A05B-6D652FB2ACE7/ISillyCalc_WSC_1/addRespo" +
            "nse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(MessageBody))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ErrorWrapper))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(CurrencyWrapper))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(DBNull))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        calctest.ServiceReference1.addResponse add(calctest.ServiceReference1.addRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.19115")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/Message")]
    public partial class MessageBody : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Xml.XmlElement[] anyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order=0)]
        public System.Xml.XmlElement[] Any {
            get {
                return this.anyField;
            }
            set {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.19115")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/System.Runtime.InteropServices")]
    public partial class ErrorWrapper : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int m_ErrorCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int m_ErrorCode {
            get {
                return this.m_ErrorCodeField;
            }
            set {
                this.m_ErrorCodeField = value;
                this.RaisePropertyChanged("m_ErrorCode");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.19115")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/System.Runtime.InteropServices")]
    public partial class CurrencyWrapper : object, System.ComponentModel.INotifyPropertyChanged {
        
        private decimal m_WrappedObjectField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public decimal m_WrappedObject {
            get {
                return this.m_WrappedObjectField;
            }
            set {
                this.m_WrappedObjectField = value;
                this.RaisePropertyChanged("m_WrappedObject");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.19115")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/System")]
    public partial class DBNull : object, System.ComponentModel.INotifyPropertyChanged {
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="add", WrapperNamespace="http://tempuri.org/DE10BAE6-FC43-3BA6-A05B-6D652FB2ACE7", IsWrapped=true)]
    public partial class addRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/DE10BAE6-FC43-3BA6-A05B-6D652FB2ACE7", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public object op1;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/DE10BAE6-FC43-3BA6-A05B-6D652FB2ACE7", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public object op2;
        
        public addRequest() {
        }
        
        public addRequest(object op1, object op2) {
            this.op1 = op1;
            this.op2 = op2;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="addResponse", WrapperNamespace="http://tempuri.org/DE10BAE6-FC43-3BA6-A05B-6D652FB2ACE7", IsWrapped=true)]
    public partial class addResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/DE10BAE6-FC43-3BA6-A05B-6D652FB2ACE7", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public object addResult;
        
        public addResponse() {
        }
        
        public addResponse(object addResult) {
            this.addResult = addResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISillyCalc_WSC_1Channel : calctest.ServiceReference1.ISillyCalc_WSC_1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SillyCalc_WSC_1Client : System.ServiceModel.ClientBase<calctest.ServiceReference1.ISillyCalc_WSC_1>, calctest.ServiceReference1.ISillyCalc_WSC_1 {
        
        public SillyCalc_WSC_1Client() {
        }
        
        public SillyCalc_WSC_1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SillyCalc_WSC_1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SillyCalc_WSC_1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SillyCalc_WSC_1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        calctest.ServiceReference1.addResponse calctest.ServiceReference1.ISillyCalc_WSC_1.add(calctest.ServiceReference1.addRequest request) {
            return base.Channel.add(request);
        }
        
        public object add(object op1, object op2) {
            calctest.ServiceReference1.addRequest inValue = new calctest.ServiceReference1.addRequest();
            inValue.op1 = op1;
            inValue.op2 = op2;
            calctest.ServiceReference1.addResponse retVal = ((calctest.ServiceReference1.ISillyCalc_WSC_1)(this)).add(inValue);
            return retVal.addResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://schemas.microsoft.com/2006/04/mex", ConfigurationName="ServiceReference1.IMetadataExchange")]
    public interface IMetadataExchange {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://schemas.xmlsoap.org/ws/2004/09/transfer/Get", ReplyAction="http://schemas.xmlsoap.org/ws/2004/09/transfer/GetResponse")]
        System.ServiceModel.Channels.Message Get(System.ServiceModel.Channels.Message request);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMetadataExchangeChannel : calctest.ServiceReference1.IMetadataExchange, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MetadataExchangeClient : System.ServiceModel.ClientBase<calctest.ServiceReference1.IMetadataExchange>, calctest.ServiceReference1.IMetadataExchange {
        
        public MetadataExchangeClient() {
        }
        
        public MetadataExchangeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MetadataExchangeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MetadataExchangeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MetadataExchangeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.ServiceModel.Channels.Message Get(System.ServiceModel.Channels.Message request) {
            return base.Channel.Get(request);
        }
    }
}
