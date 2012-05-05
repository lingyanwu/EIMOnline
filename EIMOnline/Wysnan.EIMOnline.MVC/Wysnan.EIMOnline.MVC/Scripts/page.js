/// <reference path="/Scripts/jquery-1.7.2.min.js" />

$(document).ready(function () {
    $("#imgSet").click(function () {
        if ($(".div_tool").width() == 200) {
            $(".div_tool").animate({ width: "20",
                height: "20"
            }, "slow");
        } else {
            $(".div_tool").animate({ width: "200",
                height: "650"
            }, "slow");
        }
    });
    $("#img_leftSet").click(function () {
        if ($(".div_left").width() == 250) {
            $(".div_left").animate({ width: "20"
            }, "slow");
            $(".div_right").animate({ width: "1543" }, "slow");
        } else {
            $(".div_left").animate({ width: "250"
            }, "slow");
            $(".div_right").animate({ width: "1313" }, "slow");
        }
    });

    //获取当前url，如果有#请求的页面，则跳转#号后地址
    var url = window.location.href;
    var length = url.indexOf("#");
    if (length > 0) {
        var gotoUrl = url.substring(length + 1);
        //此处需要利用ajax，根据gotoUrl获取当前模块具体信息，调用下面方法
        Navigation("aaa", "bbbb", gotoUrl, "ccc");
    }
});
function InitPage() {
    if (GlobalObj.page == null) {
        var pages = new Array();
        var page = new Page();
        page.name = "add";
        page.url = window.location.href;
        pages.push(page);
    }
}
var GlobalObj =
    {
        'pages': new Array(),
        'AddPage': function (obj) {
            $("#li_index").remove();
            var isExist = false;
            $(this.pages).each(function (i, n) {
                if (n.id == obj.id) {
                    isExist = true;
                }
            });
            if (isExist === false) {
                $("li[id*='li_wc_']").hide();
                $("#WidgetColumn").append("<li id=\"li_wc_" + obj.id + "\"></li>");
                $("#li_wc_" + obj.id).load(obj.url);

                var div = "<div class=\"pageItemDiv\" id=\"item_" + obj.id + "\"><div class=\"pageItem\" onclick=\"ShowThis('" + obj.id + "','" + obj.url + "')\">" + obj.name + "</div><div class=\"pageItemClose\" onclick=\"ItemClose('" + obj.id + "')\">×</div></div>";
                $(".div_foot").append(div);
                this.pages.push(obj);
            }
        },
        'RemovePage': function (id) {
            var obj = $("#li_wc_" + id); //获取li
            var display = obj.css("display");
            obj.remove(); //移除li
            $("#item_" + id).remove(); //移除div
            if (display != "none") {
                obj = $("li[id*='li_wc_']").first();
                obj.show(); //显示第一个
            }
            //移除数组
            var index = 0;
            $(this.pages).each(function (i, n) {
                if (n.id == id) {
                    index = i;
                }
            });
            this.pages.splice(index, 1);
        }
    };
function Page(name) {
    this.id = 0;
    this.name = name;
    this.url = null;
    this.image = null
};

function Navigation(id, name, url, image) {
    var page = new Page();
    page.id = id;
    page.name = name;
    page.url = url;
    page.image = image;
    GlobalObj.AddPage(page);
    try {
        history.pushState(null, "123", url);
    } catch (e) {

    }
}

function ItemClose(name, url) {
    GlobalObj.RemovePage(name, url);
}

function ShowThis(id, url) {
    var liId = "li_wc_" + id;
    $("li[id*='li_wc_']").hide();
    $("#" + liId).show();
    try {
        history.pushState(null, "123", url);
    } catch (e) {

    }
}
