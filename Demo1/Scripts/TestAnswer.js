$(document).ready(function () {
    $('pre code').each(function (i, block) {
        hljs.highlightBlock(block);
    });
});

//倒计时
var kaishi = document.getElementById("startTime").value;  // $("#startTime").val();
var jieshu = document.getElementById("endTime").value;// $("#endTime").val();
var time_now_server, time_now_client, time_end, time_server_client, timerID, isTrue = true, score;
time_end = new Date(jieshu);//结束的时间
time_end = time_end.getTime();
time_now_server = new Date(kaishi);//开始的时间
time_now_server = time_now_server.getTime();

time_now_client = new Date();
time_now_client = time_now_client.getTime();

time_server_client = time_now_server - time_now_client;

//改变边框
function liBorder(id) {
    var flag = $("input[name='answerIteam-" + id + "']:checked").val();
    //alert(flag);
    if (flag != null) {
        document.getElementById("S_" + id).style.border = "1px solid #0088cc";
    } else {
        document.getElementById("S_" + id).style.border = "1px solid #ddd";
    }
}
function show_time(Stop) {
    if (Stop == 1) {
        isTrue = false;
    }
    var timer = document.getElementById("timer");
    if (!timer) {
        return;
    }
}
//label hover
$(function () {
    $("label[name='labelanswer']").hover(function () {
        $(this).css("background", "#0088cc");
        $(this).css("color", "#fff");
        $(this).css("cursor", "pointer");
    }, function () {
        $(this).css("color", "#000");
        $(this).css("background", "white");
    });
});

$(function () {
    $("input[name='answerIteam']").hover(function () {
        $(this).css("cursor", "pointer");
    });
});

$(function () {
    show_time(1);//关闭倒计时
    $.ajax({
        type: "post",
        url: "../TestAnswer",
        data: {
            test_id: $("#test_id").val() * 1,
            userid: $("#userid").val() * 1
        },
        datatype: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                console.log("所选答案" + data[i].UserAnswer + "正确答案" + data[i].RealAnswer + "结果" + data[i].IsTrue);
                if (data[i].IsTrue == true) {
                    $("#ATrue-" + (i + 1) + "-" + (i + 1) + "").html("回答正确！答案为：" + "<strong style='font-size:30px;'>" + data[i].RealAnswer) + "</strong>";
                    $("#ATrue-" + (i + 1) + "").css('display', 'block');
                    document.getElementById("S_" + (i + 1)).style.border = "1px solid #0f0";
                } else {
                    $("#AFalse-" + (i + 1) + "-" + (i + 1) + "").html("正确答案：" + "<strong style='font-size:30px;'>" + data[i].RealAnswer) + "</strong>";
                    $("#AFalse-" + (i + 1) + "").css('display', 'block');
                    document.getElementById("S_" + (i + 1)).style.border = "1px solid red";

                    //$("#labelanswerA-" + (i + 1) + "")
                }
                $("input[name='answerIteam-" + (i + 1) + "']").attr("disabled", "disabled");

            }
            $("#btnTijiao").attr("disabled", "disabled");
            $("#timer").attr('id', 'score');
            //$("#Score").css('display', 'block');
            $("#score").text(Math.round(data[0].Score) + " 分");
            //console.log("总分" + Math.round(sumScore));
        }
    })
});