(function ($) {
    // ****** Add ikr.notification.css ******
    $.fn.ikrNotificationSetup = function (options) {
        /*
          Declaration : $("#noti_Container").ikrNotificationSetup({
                    List: objCollectionList
          });
       */
        var defaultSettings = $.extend({
            BeforeSeenColor: "#ffffff",
            AfterSeenColor: "#ffffff"
        }, options);
        $(".ikrNoti_Button").css({
            "background": defaultSettings.BeforeSeenColor
        });
        var parentId = $(this).attr("id");
        if ($.trim(parentId) != "" && parentId.length > 0) {
            $("#" + parentId).append("<div class='ikrNoti_Counter'></div>" +
                "<div class='ikrNoti_Button'><i class = 'fas fa-bell fa-2x' /></div>" +
                "<div class='ikrNotifications'>" +
                "<h3>Notifications (<span class='notiCounterOnHead'>0</span>)</h3>" +
                "<div class='ikrNotificationItems'>" +
                "</div>" +
                "<div class='ikrSeeAll'><a href='#'><p>See All</p></a></div>" +
                "</div>");

            $('#' + parentId + ' .ikrNoti_Counter')
                .css({ opacity: 0 })
                .text(0)
                .css({ top: '-10px' })
                .animate({ top: '19px', opacity: 1 }, 500);

            $('#' + parentId + ' .ikrNoti_Button').click(function () {
                $('#' + parentId + ' .ikrNotifications').fadeToggle('fast', 'linear', function () {
                    if ($('#' + parentId + ' .ikrNotifications').is(':hidden')) {
                        $('#' + parentId + ' .ikrNoti_Button').css('background-color', defaultSettings.AfterSeenColor);
                    }
                    else $('#' + parentId + ' .ikrNoti_Button').css('background-color', defaultSettings.BeforeSeenColor);
                });
                $('#' + parentId + ' .ikrNoti_Counter').fadeOut('slow');
                return false;
            });
            $(document).click(function () {
                $('#' + parentId + ' .ikrNotifications').hide();
                if ($('#' + parentId + ' .ikrNoti_Counter').is(':hidden')) {
                    $('#' + parentId + ' .ikrNoti_Button').css('background-color', defaultSettings.AfterSeenColor);
                }
            });
            $('#' + parentId + ' .ikrNotifications').click(function () {
                return false;
            });

            $("#" + parentId).css({
                position: "relative"
            });
        }
    };
    $.fn.ikrNotificationCount = function (options) {
        /*
          Declaration : $("#myComboId").ikrNotificationCount({
                    NotificationList: [],
                    NotiFromPropName: "",
                    ListTitlePropName: "",
                    ListBodyPropName: "",
                    ControllerName: "Notifications",
                    ActionName: "AllNotifications"
          });
       */
        var defaultSettings = $.extend({
            NotificationList: [],
            NotiFromPropName: "",
            ListTitlePropName: "",
            ListBodyPropName: "",
            ControllerName: "Notifications",
            ActionName: "AllNotifications"
        }, options);
        var parentId = $(this).attr("id");
        if ($.trim(parentId) != "" && parentId.length > 0) {
            $("#" + parentId + " .ikrNotifications .ikrSeeAll").click(function () {
                window.open('../' + defaultSettings.ControllerName + '/' + defaultSettings.ActionName + '', '_blank');
            });

            var totalUnReadNoti = defaultSettings.NotificationList.filter(k => k.stateRead == false).length;
            $('#' + parentId + ' .ikrNoti_Counter').text(totalUnReadNoti);
            $('#' + parentId + ' .notiCounterOnHead').text(totalUnReadNoti);
            if (defaultSettings.NotificationList.length > 0) {
                $.map(defaultSettings.NotificationList, function (item) {
                    var className = item.stateRead ? "" : " ikrSingleNotiDivUnReadColor";
                    var sNotiFromPropName = $.trim(defaultSettings.NotiFromPropName) == "" ? "" : item[ikrLowerFirstLetter(defaultSettings.NotiFromPropName)];
                    $("#" + parentId + " .ikrNotificationItems").append("<div class='ikrSingleNotiDiv" + className + "' notiId=" + item.notiId + ">" +
                        "<div class='ikrNotificationBody'>" + item[ikrLowerFirstLetter(defaultSettings.ListBodyPropName)] + "</div>" +
                        "<div class='ikrNofiCreatedDate'>" + item.timeSending + "</div>" +
                        "</div>");
                    $("#" + parentId + " .ikrNotificationItems .ikrSingleNotiDiv[notiId=" + item.notiId + "]").click(function () {
                        if ($.trim(item.url) != "") {
                            window.location.href = item.url;
                        }
                    });
                });
            }
        }
    };
}(jQuery));

function ikrLowerFirstLetter(value) {
    return value.charAt(0).toLowerCase() + value.slice(1);
}

