﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WheelTest {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resource1 {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource1() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WheelTest.Resource1", typeof(Resource1).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性，对
        ///   使用此强类型资源类的所有资源查找执行重写。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 using System;
        ///using System.Collections.Generic;
        ///using System.Text;
        ///using NewGTA.Modules.Shared;
        ///using StockPre.Domain;
        ///namespace Modules.StockPre.Application.Ensure
        ///{
        ///    public class Ensure&amp;Command : IEnsureCacheCommand&lt;IEnumerable&lt;&amp;&gt;&gt;
        ///    {
        ///        
        ///        public &amp;[] Orders { get; private set; }
        ///
        ///        public Ensure&amp;Command(params &amp;[] orders)
        ///        {
        ///            this.Orders = orders;
        ///        }
        ///    }
        ///}
        /// 的本地化字符串。
        /// </summary>
        public static string Command {
            get {
                return ResourceManager.GetString("Command", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 using System;
        ///using System.Collections.Generic;
        ///using System.Text;
        ///using System.Threading;
        ///using System.Threading.Tasks;
        ///using MediatR;
        ///using NewGTA.Core.Persistence;
        ///using StockPre.Domain;
        ///
        ///namespace Modules.StockPre.Application.Ensure
        ///{
        ///    public class Ensure&amp;Handler : IRequestHandler&lt;Ensure&amp;Command, IEnumerable&lt;&amp;&gt;&gt;
        ///    {
        ///        private readonly IRepository&lt;&amp;&gt; _orderRepository;
        ///        public Ensure&amp;Handler(IRepository&lt;&amp;&gt; orderRepository)
        ///        {
        ///            _orderRepository = orderRep [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        public static string Handler {
            get {
                return ResourceManager.GetString("Handler", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似  的本地化字符串。
        /// </summary>
        public static string String1 {
            get {
                return ResourceManager.GetString("String1", resourceCulture);
            }
        }
    }
}