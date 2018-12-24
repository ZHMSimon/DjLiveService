namespace DjLive.SdkModel.Enum
{
    public enum ExceptionType
    {
        None = 0,
        AccessDenied = 1,                                  //不可以使用音视频直播服务，请联系工单开通	
        UserOverdue,                                   //用户账号欠费，请续费
        InvalidSession,                                //创建流参数错误	
        NoSuchSession,                                 //流不存在	
        OperationNotPermitted,                         //非法操作
        InvalidPreset,                                 //创建模板参数错误
        DuplicatePreset,                               //模板名称已被使用	
        NoSuchPreset,                                  //模板不存在
        DeleteActiveResource,                          //资源使用中，无法删除	
        InvalidSecurityPolicy,                         //更新模板参数错误
        DuplicateSecurityPolicy,                       //安全策略名称已被使用
        NoSuchSecurityPolicy,                          //安全策略不存在
        SecurityPolicyInUse,                           //安全策略使用中，无法更新	
        InvalidNotification,                           //创建通知参数错误
        DuplicateNotification,                         //通知名称已被使用
        NoSuchNotification,                            //通知不存在	
        NotificatioInUse,                              //通知使用中，无法删除	
        UnExceptError,
        UnDefinedError,
        ReadyForProcessError,
    }
}
