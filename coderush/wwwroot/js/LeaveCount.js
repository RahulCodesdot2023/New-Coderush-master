$(document).ready(function () {
    $('#btninsert').click(function () {
        var userid = "";
        if ($("#txtRole").val() == "SuperAdmin" || $("#txtRole").val() == "HR") {
            userid = $("#leavedrpdwn").val();
        } else {
            userid = $("#txtUserId").val();
        }

        var id = $("#hednid").val();
        var fromdate = $("#levfrmdate").val();
        var todate = $("#levtodate").val();
        var count = $("#levcount").val();
        var EmployeeDescription = $("#levdescription").val();
        var HrDescription = $("#HrDescription").val();
        var isapprove = $("#isaprv").val();
        var approveDate = $("#apprvdate").val();
        var formdata = new FormData();
        formdata.append('Id', id);
        formdata.append('Userid', userid);
        formdata.append('Fromdate', fromdate);
        formdata.append('Todate', todate);
        formdata.append('Count', count);
        formdata.append('EmployeeDescription', EmployeeDescription);
        formdata.append('HrDescription', HrDescription);
        formdata.append('Isapprove', isapprove);
        formdata.append('ApproveDate', approveDate);
        $.ajax({
            url: '/Leavecount/SubmitForm',
            type: "POST",
            contentType: false,
            processData: false,
            data: formdata,
            success: function (result) {
                toastr.success(result.message);
                Bindtablegrid(userid, "");
            },
            error: function (err) {
                toastr.error("Fail!");
            }
        });
    });

    //$('#grid').DataTable({
    //    lengthChange: false,
    //    info: false,
    //    searching: true,
    //    dom: 'lrtip',
    //    scrollX: false,
    //    pageLength: 25,
    //});

    $("#btninsert").unbind().click(function () {
        var userid = $("#leavedrpdwn").val();
        if (userid == null || userid == "" || userid == undefined) {
            /* alert("Please select drowpdown menu first !!");*/

            return false;
        }
        $(".text-danger").hide();
    });

    var Bindtablegrid = function (id, username) {
        if (id == null || id == '' || id == undefined) {
            id = $("#txtUserId").val();
        }
        var isRole = JSON.parse($("#txtRole").val().toLowerCase());

        $.ajax({
            url: "/Leavecount/BindGridData?id=" + id,
            method: 'Get',
            data: {},
            success: function (data) {

                console.log(data);
                $("#levtblbdy").empty();
                if (data.list != null && data.list.length > 0) {
                    var innerHtml = '';
                    var arrya = new Array();
                    var totalLeavesCount = 0;
                    var CurrentDate = new Date();
                    $.each(data.list, function (i, v) {
                        var rowdata = data.list[i]
                        innerHtml += "<tr style='background-color:" + rowdata.colouris + "'>";
                        //innerHtml += "<td><i class='fa fa-edit' style='font-size:20px' id='btnedit' data-id = " + rowdata.id + "></i></td>";
                        //innerHtml += "<td><i class='fa fa-trash' ></i></td>";
                        //innerHtml += "<td><a href='/Leavecount/Form?id=" + rowdata.id + "'&userid='" + rowdata.userid +"'><i class='fa fa-edit'></i></a></td>";
                        //innerHtml += "<td scope='col' id=''>" + rowdata.id + "</td>";
                        if (rowdata.isedit) {
                            //innerHtml += "<td><a href='javascript:void(0)' onclick='editForm(); return false;' class='editleave'data-id='" + rowdata.id + "'data-userid='" + rowdata.userid + "'><i class='fa fa-edit'></i></a></td>";
                            var GivenDate = new Date(rowdata.todate);
                            if (CurrentDate < GivenDate && rowdata.isapprove == false && (rowdata.isReject == false || rowdata.isReject == null)) {
                                //innerHtml += "<td><a href='javascript:void(0)' onclick=editForm('" + rowdata.userid + "'," + rowdata.id + "); class='editleave' data-id='" + rowdata.id + "'data-userid='" + rowdata.userid + "'><i class='fa fa-edit'></i></a></td>";
                                innerHtml += "<td><button onclick=editForm('" + rowdata.userid + "'," + rowdata.id + "); class='editleave'><i class='fa fa-edit'></i></button></td>";
                            } else {
                                innerHtml += "<td><button disabled='disabled'><i class='fa fa-edit'></i></button></td>";
                            }
                        }
                        else {
                            innerHtml += "<td><button disabled='disabled'><i class='fa fa-edit'></i></button></td>";
                        }
                        //innerHtml += "<td><a href='javascript:void(0)' class='editleave'data-id='" + rowdata.id + "'data-userid='" + rowdata.userid + "'><i class='fa fa-edit'></i></a></td>";
                        //innerHtml += "<td><a href='/Leavecount/Delete/" + rowdata.id + "'><i class='fa fa-trash'></i></a></td>";
                        innerHtml += "<td scope='col' id='User ID'>" + rowdata.fullName + "</td>";
                        innerHtml += "<td scope='col' id='From Date'>" + moment(rowdata.fromdate).format('LL') + "</td>";
                        //harshal
                        innerHtml += "<td scope='col' id='To Date'>" + moment(rowdata.todate).format('LL') + "</td>";
                        innerHtml += "<td scope='col' id='Count'>" + rowdata.count + "</td>";
                        innerHtml += "<td scope='col' id='LeaveReason'>" + rowdata.leaveVal + "</td>";
                        innerHtml += "<td scope='col' id='empDescription'>" + rowdata.employeeDescription + "</td>";
                        if (isRole == true) {
                            innerHtml += "<td scope='col' id='Description'>" + rowdata.hrDescription + "</td>";
                        }
                        /*innerHtml += "<td scope='col' id='IsApprove'>" + rowdata.isapprove + "</td>";*/
                        if (rowdata.isapprove == true) {
                            innerHtml += "<td scope='col' > <input type='checkbox' data-approveId='1' class='clsChecked' id='IsChecked" + rowdata.id + "' disabled /></td>";
                            arrya.push(rowdata.id);
                        } else {
                            innerHtml += "<td scope='col' > <input type='checkbox' data-approveId='0' class='clsChecked' id='IsChecked" + rowdata.id + "' disabled /></td>";
                        }
                        innerHtml += "<td scope='col' id='Created Date'>" + moment(rowdata.createdDate).format('LL') + "</td>";
                        //innerHtml += '<td>@Html.ActionLink("Download", "DownloadFile", new { fileName = item.FileUpload })</td>';
                        if (rowdata.filename != null && rowdata.filename != "") {

                            innerHtml += "<td><a class='cladownload' data-downloadFile='/document/Leave/" + rowdata.filename + "'><i class='fa fa-download'></i></a></td>";
                        }
                        else {
                            innerHtml += "<td></td>"

                        }
                        if (isRole != false) {
                            if ((rowdata.isReject == null || rowdata.isReject == false) && rowdata.isapprove == false) {
                                var GivenDate = new Date(rowdata.todate);
                                if (CurrentDate < GivenDate) {
                                    innerHtml += "<td><input type='button' value='&#10004;' class='clsAprove btn btn-success pull-left' data-aproveid='" + rowdata.id + "' id='btn_" + rowdata.id + "' >&nbsp;<input type='button' value='&#10008;' class='clsReject btn btn-danger' data-aproveid='" + rowdata.id + "' id='btn_" + rowdata.id + "' ></td>";
                                    //innerHtml += "<td><input type='button' value='Reject' class='clsAprove btn btn-reject' data-aproveid='" + rowdata.id + "' id='btn_" + rowdata.id + "' ></td>";
                                } else {
                                    innerHtml += "<td>-</td>";
                                }
                            }
                            else {
                                if (rowdata.isapprove == true) {
                                    innerHtml += "<td><b style='color: green;'>APPROVED</b></td>";
                                } else {
                                    innerHtml += "<td><b style='color: red;'>REJECTED</b></td>";
                                }
                            }
                        }
                        var apprDate = "-";
                        if (rowdata.approveDate) { apprDate = rowdata.approveDate; }
                        innerHtml += "<td>" + apprDate + "</td>";
                        innerHtml += "</tr>"
                        totalLeavesCount += parseInt(rowdata.count);

                    });

                    var totalYearlyLeave = parseInt($("#lblYearlyLeaves").text());
                    var total = 0;

                    $("#levtblbdy").html(innerHtml);
                    $("#lblTotalLeavesCount").text(totalLeavesCount);
                    total = (totalYearlyLeave - totalLeavesCount);
                    $("#lblRemainingLeaves").text(total);
                    gridSorting();


                    for (var i = 0; i < arrya.length; i++) {
                        console.log(arrya[i]);
                        $('#IsChecked' + arrya[i]).prop('checked', true);
                    }
                }
                else {
                    var innerhtml = '<tr><td colspan="9" class="text-center">No record found.</td></tr>';
                    $("#levtblbdy").html(innerhtml);
                }
            }
        });


    }

    function gridSorting() {
        $("#grid").dataTable({
            aaSorting: [[2, 'asc']],
            bPaginate: false,
            bFilter: false,
            bInfo: false,
            bSortable: true,
            bRetrieve: true,
            aoColumnDefs: [
                { "aTargets": [0], "bSortable": true },
                { "aTargets": [1], "bSortable": true },
                { "aTargets": [2], "bSortable": true },
                { "aTargets": [3], "bSortable": true },
                { "aTargets": [4], "bSortable": true },
                { "aTargets": [5], "bSortable": true },
                { "aTargets": [6], "bSortable": true },
                { "aTargets": [7], "bSortable": true }
            ]
        });
    }

    $(document).on("click", ".clsAprove", function () {
        var Id = $(this).data('aproveid');
        $('#myModal').modal('show');
        $("#leaveId").val(Id);
        clearTextBox();
        $('#IsApprove').prop('checked', true);
        $('#IsReject').prop('disabled', true);
    })

    $(document).on("click", ".clsReject", function () {
        var Id = $(this).data('aproveid');
        $('#myModal').modal('show');
        $("#leaveId").val(Id);
        clearTextBox();
        $('#IsReject').prop('checked', true);
        $('#IsApprove').prop('disabled', true);
    })

    $(document).on("click", ".Approvcheck", function () {
        $('.Approvcheck').not(this).prop('checked', false);
    })

    $(document).on("click", "#cancelpopup", function () {
        $('#myModal').modal('hide');
    })

    $(document).on("click", "#saveLeave", function () {
        var Id = $('#leaveId').val();
        var HRDescription = $('#notes').val();
        if (HRDescription == '') {
            toastr.warning("HRDescription is Required");
            return false;
        }
        var Isapprove = $('.Approvcheck:checked').val() == "1" ? true : false;
        $.ajax({
            url: "/Leavecount/leavepopuop?Id=" + Id + "&HRDescription=" + HRDescription + "&Isapprove=" + Isapprove,
            type: "POST",
            contentType: false,
            processData: false,
            contentType: "application/json",
            success: function (res) {
                var id = $("#leavedrpdwn option:selected").val();
                var username = $("#leavedrpdwn option:selected").text();
                Bindtablegrid(id, username);
            },
            error: function (res, err) {
            }
        });
    })

    var BinddrpdwnData = function () {
        $.ajax({
            url: "/Leavecount/BinddrpdwnData",
            type: "GET",
            contentType: false,
            processData: false,
            contentType: "application/json",
            success: function (res) {
                var Project = "#leavedrpdwn";
                $(Project).empty();
                $(Project).append($("<option></option>").val("").html("--Select--"));
                $.each(res, function (index, object) {
                    $(Project).append($("<option></option>").val(object.value).html(object.text));
                    //Bindtablegrid(object.value, object.text);
                });
            },
            error: function (res, err) {

            }
        });
    };

    $("#leavedrpdwn").on('change', function () {
        var id = $("#leavedrpdwn option:selected").val();
        var username = $("#leavedrpdwn option:selected").text();
        Bindtablegrid(id, username);
        $("#grid").show();
    });
    $("#btninsert").unbind().click(function () {
        var userid = "";
        if ($("#txtRole").val() == false) {
            userid = $("#leavedrpdwn").val();
        } else {
            userid = $("#txtUserId").val();
        }

        //var userid = $("#leavedrpdwn").val();
        if (userid == null || userid == "" || userid == undefined) {

            var username = $("#leavedrpdwn option:selected").text();
            toastr.warning("Please select user");
            return false;
        }


        var userid = $("#leavedrpdwn").val();
        window.location.href = '/LeaveCount/Form?id=' + 0 + '&userid=' + userid;
    });

    $('body').on('click', '#btnedit', function () {
        var id = $(this).data('id');
        $.ajax({
            url: '/Leavecount/Form?id=' + id,
            type: "GET",
            contentType: false,
            processData: false,
            success: function (result) {
                $("#hednid").val(result.id);
                $("#levuserid").val($("#leavedrpdwn option:selected").text());
                $("#levfrmdate").val(result.fromdate);
                $("#levtodate").val(result.todate);
                $("#levcount").val(result.count);
                $("#levdescription").val(result.EmployeeDescription);
                $("#isaprv").val(result.isapprove);
                $("#apprvdate").val(result.approveDate);
                $("#AddLeave").modal("show");
                $(".text-danger").hide();
            },
            error: function (err) {
            }
        });
    });

    BinddrpdwnData();
    Bindtablegrid("", "");

    $(document).on("click", ".cladownload", function () {
        var fileName = $(this).attr('data-downloadFile');
        //var path = "/document/Leave/";
        window.open(window.origin + fileName, '_blank'); // open the pdf in a new window/tab
    })

    function clearTextBox() {
        $('#notes').val("");
        $('#notes').css('border-color', 'lightgrey');
        $('#IsApprove').css('border-color', 'lightgrey');
        $('#IsReject').css('border-color', 'light');
        $("#IsApprove").prop("checked", false);
        $("#IsReject").prop("checked", false);
    }
    // var totalYearsLeaves = 200;
    var leavCountuserId = $("#leavcountuserId").val();

    Bindtablegrid(leavCountuserId, "");
});


function editForm(userids, leaveid) {
    window.location.href = '/LeaveCount/Form?id=' + leaveid + '&userid=' + userids;
}