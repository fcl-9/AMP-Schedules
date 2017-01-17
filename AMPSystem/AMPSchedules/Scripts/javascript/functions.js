// Overides some configurations of validate to use bootstrap error
function configureValidator() {
    $.validator.setDefaults({
        highlight: function(element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function(element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function(error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });
}

//Makes Request to the server - Generic
function requestGetType(requesturl, data) {

    if (viewCalendar) {
        data["start"] = $("#calendar").fullCalendar("getView").start.format();
        data["end"] = $("#calendar").fullCalendar("getView").end.format();
    } else {
        //If i'm in next event view just do other thing
        data["start"] = moment($("#startTime").text(), "Do MMM YYYY H:mm").subtract(7, "days").format("YYYY-MM-DD HH:mm:ss");
        data["end"] = moment($("#endTime").text(), "Do MMM YYYY H:mm").add(7, "days").format("YYYY-MM-DD HH:mm:ss");
    }

    $.ajax({
        type: "GET",
        url: requesturl,
        data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (events) {
            location.reload();
        },
        failure: function (response) {
            console.log("Fail");
            alert(response.d);
        }
    });
}


//Get my courses
function getCourses(functionRender) {
    $.ajax({
        type: "GET",
        url: "/Courses",
        success: functionRender,
        failure: function (response) {
            console.log("Fail");
            alert(response.d);
        }
    });

}

//Get Every Room 
function getRooms(functionRender) {
    $.ajax({
        type: "GET",
        url: "/Buildings",
        success: functionRender,
        failure: function (response) {
            console.log("Fail");
            alert(response.d);
        }
    });
}

//Return a Json with active alerts
function getActiveAlerts(item, funtionSucess) {
    $.ajax({
        type: "GET",
        url: "/Alert/Index",
        data: item,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: funtionSucess,
        failure: function (response) {
            console.log("Fail");
            alert(response.d);
        }
    });
}

//Adding Rooms 
function addRoomToEventAdder(rooms) {
    $("#new_event_room").html(function () {
        var room = "<option value=''>Select...</option>";
        $.each(rooms,
            function (property, value) {
                $.each(value.Rooms,
                    function (key, val) {
                        room += "<option value='" +
                            value.Name +
                            " - " +
                            val.Name +
                            "'>" +
                            value.Name +
                            " - " +
                            val.Name +
                            "</option>";
                    });
            });
        return room;
    });
}

//Add Courses to SelectBox in Event Addition
function addCourseToEventAdder(courses) {
    $("#new_event_course")
        .html(function () {
            var mCourses = "<option value=''>Select...</option>";
            $.each(courses,
                function (property, value) {
                    mCourses += "<option value='" +
                        value.Name +
                        "'>" +
                        value.Name +
                        "</option>";
                });
            return mCourses;
        });
}

//Renders the calendar.
function calendar(urlToRequestData) {
    // page is now ready, initialize the calendar...
    $('#calendar')
        .fullCalendar({
            // put your options and callbacks here
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            navLinks: true, // can click day/week names to navigate views
            editable: true,
            eventLimit: true, // allow "more" link when too many events
            events: "" + urlToRequestData + "",
            eventClick: renderEventModal
        });
}

//Modal Info Screen
function renderEventModal(event) {
    currentEvent = event;
    if (!event.editable) {
        $("#removeEvent").attr("disabled", "disabled");
    } else {
        $("#removeEvent").removeAttr("disabled");
    }
    $("#modalTitle").html(event.title);
    $("#startTime").html(moment(event.start).format("Do MMM YYYY H:mm"));
    $("#endTime").html(moment(event.end).format("Do MMM YYYY H:mm"));
    $("#rooms")
        .html(function () {
            var mRooms = "Room(s): <br><ul>";
            $.each(event.rooms,
                function(key, room) {
                    mRooms += "<li>" +
                        room.Name +
                        "<ul><li> Floor: " +
                        room.Floor +
                        "</li><li>Building: " +
                        room.Building.Name +
                        "</li></ul>";
                });
            mRooms += "</ul>";
            return mRooms;
        });
    if (event.teacher !== null) {

        $("#teacher").html("Teacher: " + event.teacher.Name);
        $("#lessonType").html("Lesson: " + event.lessonType);
    }
    $("#description").html(event.description);
    $("#fullCalModal").modal();
}

//Apply Filters
function applyFilters() {
    var selectedFilter = $("input[type=checkbox]");
    var selectedElement = undefined;
    var filterToApply = {};

    selectedFilter.click(function () {
        selectedElement = $(this);
        if ($(this).is(":checked")) {
            filterToApply[$(this).val()] = $(this).attr('name');

            //All filters from the same type will be bocked
            selectedFilter.each(function () {
                if ($(this).attr("name") === selectedElement.attr("name") &&
                    $(this).val() !== selectedElement.val()) {
                    $(this).attr("disabled", true);
                }
            });


        } else {
            var valueToRemove = $(this).val();
            delete filterToApply[valueToRemove];
            console.log(filterToApply);

            selectedFilter.each(function () {
                if ($(this).attr("name") === selectedElement.attr("name") &&
                    $(this).val() !== selectedElement.val()) {
                    $(this).removeAttr("disabled");
                }
            });
        }
        //Ajax Request
        requestGetType("/Filters/Add", filterToApply);
    });

}

//Render CheckBox Filters
function renderFilterCheckBoxes(courses) {
    $("#nameFilters")
        .html(function () {
            var mCourses = "";
            $.each(courses,
                function (property, value) {
                    mCourses += "" +
                        "<div class='checkbox'>" +
                        "<label>" +
                        "<input type='checkbox' name='ClassName' value='" +
                        value.Name +
                        "'>" +
                        value.Name +
                        "</label>" +
                        "</div>";
                });
            return mCourses;
        });
    applyFilters();
}

//Removes one alert from the database
function removeAnAlert(datatoSend, onSuccess) {
    $.ajax({
        type: "GET",
        url: "/RemoveAlert/Index",
        data: datatoSend,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: onSuccess,
        failure: function (response) {
            console.log("Fail");
            alert(response.d);
        }
    });
}

//This function will render the form used to remove active alerts
function renderActiveAlerts(activeAlerts) {
    if (activeAlerts.length === 0) {
        //There are no events
        $("#activeAlertForm").html("There are no active alerts for the selected event.");
    } else {
        $.each(activeAlerts,
            function (key, value) {
                $("#activeAlertForm").append(
                    '' +
                    '<div class="col-sm-12 form-group">' +
                    '<div class="col-sm-10">' +
                    '<input class="form-control" value="' +
                    value.Value+
                    '" readonly=""/>' +
                    '</div>' +
                    '<div class="col-sm-2">' +
                    '<button class="rm-active-alert btn btn-danger" id="' +
                    value.Key +
                    '">Remove</button>' +
                    '</div>' +
                    '</div>'
                );
            });
        //moment(value.Value, "YYYY-MM-DTHH:mm:ss").format("ddd Do MMM YYYY HH:mm")+

    }
}

//Sanitize Modal Fields
function sanitizeModalFields() {
    $("#new_event_name").val("");
    $("#datetimepicker_starttime").val("");
    $("#datetimepicker_endtime").val("");
    $("#new_event_room").val("");
    $("#new_event_course").val("");
    $("#new_event_description").val("");
}

var counter = 0; // Global Variable
//Handles all the operations over alertModal
function AlertModalFunctions() {
    $("#addAlert")
        .click(function () {
            $("#alertForm")
                .append('' +
                    '<div class="col-sm-12 form-group">' +
                    '<div class="col-sm-5">' +
                    '<input data-msg="Please insert a number" class="form-control number" name=number'+counter+'/>' +
                    '</div>' +
                    '<div class="col-sm-5">' +
                    '<select data-msg="Please insert a time unit" class="form-control timeUnit" name=timeUnit' + counter + ' >' +
                    '<option value="">Select...</option>' +
                    '<option>Minutes</option>' +
                    '<option>Hours</option>' +
                    '<option>Days</option>' +
                    '<option>Weeks</option>' +
                    '</select>' +
                    '</div>' +
                    '<div class="col-sm-2">' +
                    '<button class="rm-alert btn btn-danger">Remove</button>' +
                    '</div>' +
                    '</div>'
                );
            counter += 1;
            $(".timeUnit").each(function() {
                $(this).rules("add",
                {
                    required: true
                });
            });
            $(".number").each(function() {
                $(this).rules("add",
                {
                    required: true,
                    digits: true
                });
            });
        });

    $("#alertForm")
        .on("click",
            ".rm-alert",
            function (e) {
                e.preventDefault();
                $(this).parent().parent().remove();
            });

    $("#submit").click(function () {
        if ($("#1").hasClass("active")) {
            $('#fullCalModal').modal('hide');
        } else if ($("#2").hasClass("active")) {
            var colorApplyer = {};
            //                  Event Name                  Color Picked for the event Selected.
            colorApplyer[$('#modalTitle').text()] = $('select[name="colorpicker"]').val();
            //Request Data
            requestGetType("/Color/Add", colorApplyer);
            $('#fullCalModal').modal('hide');
        } else if ($("#3").hasClass("active")) {
            $("#alertForm").valid();
            //console.log(" " + $("#startTime").text()+ " " + $("#endTime").text());
            var times = $("#alertForm").find(".number");
            var units = $("#alertForm").find(".timeUnit");
            var alerts = {};
            var error = false;
            for (var i = 0; i < times.length; i++) {
                var alert = {};
                if ($.isNumeric($(times[i]).val()) && $(times[i]).val() !== "") {
                    alert["name"] = $("#modalTitle").text();
                    alert["startTime"] = moment($("#startTime").text(), "Do MMM YYYY H:mm").format("YYYY-MM-DD HH:mm:ss");
                    console.log(alert["startTime"]);
                    alert["endTime"] = moment($("#endTime").text(), "Do MMM YYYY H:mm").format("YYYY-MM-DD HH:mm:ss");
                    alert["time"] = $(times[i]).val();
                    alert["unit"] = $(units[i]).val();

                    alerts[i] = alert;
                } else {
                    error = true;
                    break;
                }
            }

            //This is for the time calendar
            if (viewCalendar) {
                alerts["start"] = $("#calendar").fullCalendar("getView").start.format();
                alerts["end"] = $("#calendar").fullCalendar("getView").end.format();
            } else {
                alerts["start"] = moment($("#startTime").text(), "Do MMM YYYY H:mm").subtract(7, "days").format("YYYY-MM-DD HH:mm:ss");
                alerts["end"] = moment($("#endTime").text(), "Do MMM YYYY H:mm").add(7, "days").format("YYYY-MM-DD HH:mm:ss");
            }

            if (error === false) {
                $.ajax({
                    type: "GET",
                    url: "/AddAlert/Index",
                    data: alerts,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (events) {
                        location.reload();
                    },
                    failure: function (response) {
                        console.log("Fail");
                        alert(response.d);
                    }
                });

                $("#fullCalModal").modal("hide");
            } else {
                console.log("Check the data you inputed.");
            }
        }else if ($("#5").hasClass("active")) {
            AddorEditReminder();
        }
    });

}

//When Button is Clicked open modal to add Event
function bindAddEventButton() {
    $("#addEvent").click(function () {
        $(".modal-title-addEvent").html("Add a New Event");
        getCourses(addCourseToEventAdder);
        getRooms(addRoomToEventAdder);
        $("#addEventModal").modal();
    });
}

//Handles the datepickers 
function renderDatePickers() {
    $("#datetimepicker_starttime").datetimepicker({
        sideBySide: true,
        minDate: moment().add(10, "m"),
        format: "M/D/YYYY HH:mm"
    }).on("dp.change",
        function (selected) {
            minDate = new Date(selected.date.valueOf());
            var minDatePlusTen = new Date(minDate);
            minDatePlusTen.setMinutes(minDate.getMinutes() + 10);
            //$("#datetimepicker_endtime").data("DateTimePicker").enable();
            $("#datetimepicker_endtime").data("DateTimePicker").date(minDatePlusTen);
        });

    $("#datetimepicker_endtime").datetimepicker({
        sideBySide: true,
        minDate: moment().add(20, "m"),
        format: "M/D/YYYY HH:mm"
    }).on("dp.change",
        function (selected) {
            var currentDate = new Date(selected.date.valueOf());
            var minDatePlusTen = new Date(minDate);
            minDatePlusTen.setMinutes(minDate.getMinutes() + 10);

            //Reset the date of the event to the min date of the date picker with the loest date
            if (currentDate < minDatePlusTen) {
                $("#datetimepicker_endtime").data("DateTimePicker").date(minDatePlusTen);
            }
        });

    //$("#datetimepicker_endtime").data("DateTimePicker").disable();
}

//Creates the Json and makes the request
function eventAddRequester() {
    var newEvent = {};
    //Generates the json to send to the server and store it in the database.
    var name = $("#new_event_name").val();
    var beginsAt = $("#datetimepicker_starttime").data('date');
    var endsAt = $("#datetimepicker_endtime").data('date');
    var room = $("#new_event_room").val();
    var course = $("#new_event_course").val();
    var description = $("#new_event_description").val();
    var reminder = $("#new_reminder").val();
    // Parses Data
    beginsAt = moment(beginsAt, "M/D/YYYY HH:mm");
    endsAt = moment(endsAt, "M/D/YYYY HH:mm");

    //Add Values to Json
    newEvent["title"] = name;
    newEvent["beginsAt"] = beginsAt.format();
    newEvent["endsAt"] = endsAt.format();
    newEvent["room"] = room;
    newEvent["course"] = course;
    newEvent["description"] = description;
    newEvent["reminder"] = reminder;
    console.log(newEvent);
    $("#addEventModal").modal("hide");
    sanitizeModalFields();
    if (viewCalendar) {
        newEvent["start"] = $("#calendar").fullCalendar("getView").start.format();
        newEvent["end"] = $("#calendar").fullCalendar("getView").end.format();
    } else {
        newEvent["start"] = moment(beginsAt, "Do MMM YYYY H:mm").subtract(7, "days").format("YYYY-MM-DD HH:mm:ss");
        newEvent["end"] = moment(endsAt, "Do MMM YYYY H:mm").add(7, "days").format("YYYY-MM-DD HH:mm:ss");
    }
    //Request Ajax Send to Database.
    requestEvent("/AddEvent/AddEvent", newEvent);
}

//Makes get Event
function requestEvent(requesturl, data) {
    $.ajax({
        type: "GET",
        url: requesturl,
        data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (events) {
            location.reload();
        },
        failure: function (response) {
            console.log("Fail");
            alert(response.d);
        }
    });
}

// Remove events
function removeUserAddedEvents() {
    $("#remove").click(function () {
        var data = {};
        data.id = currentEvent.id;
        data.name = currentEvent.title;
        if (viewCalendar) {
            data.startEvent = currentEvent.start.format();
            data.endEvent = currentEvent.end.format();
        } else {
            data.startEvent = currentEvent.start;
            data.endEvent = currentEvent.end;
        }
        console.log(data);
        requestGetType("/RemoveEvent/", data);
        $("#fullCalModal").modal("hide");
    });
}

//Clicks on active alerts tab number 4
function clickTabFour() {
    $('a[href="#4"]').click(function() {
        if ($("#activeAlertForm").is(":empty")) {
            //pass
        } else {
            $("#activeAlertForm").html("");
        }
        var selectedItem = {};
        selectedItem["name"] = $("#modalTitle").text();
        selectedItem["startTime"] = moment($("#startTime").text(), "Do MMM YYYY H:mm").format("YYYY-MM-DD HH:mm:ss");
        selectedItem["endTime"] = moment($("#endTime").text(), "Do MMM YYYY H:mm").format("YYYY-MM-DD HH:mm:ss");
        if (viewCalendar) {
            selectedItem["start"] = $("#calendar").fullCalendar("getView").start.format();
            selectedItem["end"] = $("#calendar").fullCalendar("getView").end.format();
        } else {
            selectedItem["start"] = moment($("#startTime").text(), "Do MMM YYYY H:mm").subtract(7, "days").format("YYYY-MM-DD HH:mm:ss");
            selectedItem["end"] = moment($("#endTime").text(), "Do MMM YYYY H:mm").add(7, "days").format("YYYY-MM-DD HH:mm:ss");
        }
        getActiveAlerts(selectedItem, renderActiveAlerts);
    });
}

//Validator Jquery Validator of the addEvent Modal
function addJqueryValidator() {
    $("#new_event-add-form").validate({
        rules: {
            new_event_name: "required",
            datetimepicker_starttime: "required",
            datetimepicker_endtime: "required",
            new_event_room: "required",
            new_event_course: "required"
        },
        messages: {
            new_event_name: "Please enter a name for the event.",
            datetimepicker_starttime: "Please select a start date.",
            datetimepicker_endtime: "Please select a end date.",
            new_event_room: "Please select a room.",
            new_event_course: "Please select a course."
        },
        submitHandler: function (form) {
            return false;
        }
    });
    $("#alertForm").validate({
        rules: {
            number: {
                required: true,
                digits:true
            },
            timeUnit: "required"
        },
        messages: {
            number: "Please enter a number",
            timeUnit: "Please select a time unit"
        },
        submitHandler: function (form) {
            return false;
        }
    });

}

//Submit the Data of a new event
function saveNewEventBtt() {
    $("#saveNewEvent").click(function () {
        if ($("#new_event-add-form").valid() === true) {
            eventAddRequester();
        } else {
            console.log("Error Ocurred Submiting");
        }
    });
}

//When button to remove an active alert is clicked
function removeActiveAlert() {
    $("#activeAlertForm").on("click",
        ".rm-active-alert",
        function (e) {
            removeAlert = {};
            removeAlert["alertId"] = $(this).attr("id");
            removeAlert["name"] = $("#modalTitle").text();
            removeAlert["startTime"] = moment($("#startTime").text(), "Do MMM YYYY H:mm").format("YYYY-MM-DD HH:mm:ss");
            removeAlert["endTime"] = moment($("#endTime").text(), "Do MMM YYYY H:mm").format("YYYY-MM-DD HH:mm:ss");
            //Removes Everything
            $("#activeAlertForm").html("");
            //Makes ajax request and receives it back
            if (viewCalendar) {
                removeAlert["start"] = $("#calendar").fullCalendar("getView").start.format();
                removeAlert["end"] = $("#calendar").fullCalendar("getView").end.format();
            } else {
                removeAlert["start"] = moment($("#startTime").text(), "Do MMM YYYY H:mm").subtract(7, "days").format("YYYY-MM-DD HH:mm:ss");
                removeAlert["end"] = moment($("#endTime").text(), "Do MMM YYYY H:mm").add(7, "days").format("YYYY-MM-DD HH:mm:ss");
            }
            removeAnAlert(removeAlert, renderActiveAlerts);
        });
}

//Ajax request to get a reminder that is associated to an item
function getActiveReminders(item, renderReminder) {
    $.ajax({
        type: "GET",
        url: "/Reminder",
        data: item,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: renderReminder,
        failure: renderReminder
    });

}

//Check for a click in tab of reminders
function clickTabFive() {
    $('a[href="#5"]').click(function () {
        var selectedItem = {};
        selectedItem["name"] = $('#modalTitle').text();
        selectedItem["startTime"] = moment($("#startTime").text(), "Do MMM YYYY H:mm")
            .format("YYYY-MM-DD HH:mm:ss");
        selectedItem["endTime"] = moment($("#endTime").text(), "Do MMM YYYY H:mm")
            .format("YYYY-MM-DD HH:mm:ss");
        if (viewCalendar) {
            selectedItem["start"] = $("#calendar").fullCalendar("getView").start.format();
            selectedItem["end"] = $("#calendar").fullCalendar("getView").end.format();
        } else {
            selectedItem["start"] = moment($("#startTime").text(), "Do MMM YYYY H:mm").subtract(7, "days").format("YYYY-MM-DD HH:mm:ss");
            selectedItem["end"] = moment($("#endTime").text(), "Do MMM YYYY H:mm").add(7, "days").format("YYYY-MM-DD HH:mm:ss");
        }
        getActiveReminders(selectedItem, renderReminderSuccess);
    });
}

//Gets the json and renders it's information on the interface
function renderReminderSuccess(activeReminder) {
        $("#display_reminder").val(activeReminder['Message']);
}

//Updates Reminders Message or Adds a New One.
function AddorEditReminder() {
            newReminder = {};
            newReminder["name"] = $('#modalTitle').text();
            newReminder["startTime"] = moment($("#startTime").text(), "Do MMM YYYY H:mm")
                .format("YYYY-MM-DD HH:mm:ss");
            newReminder["endTime"] = moment($("#endTime").text(), "Do MMM YYYY H:mm")
                .format("YYYY-MM-DD HH:mm:ss");
            newReminder["reminder"] = $("#display_reminder").val();
    if (viewCalendar) {
        newReminder["start"] = $("#calendar").fullCalendar("getView").start.format();
        newReminder["end"] = $("#calendar").fullCalendar("getView").end.format();
    } else {
        newReminder["start"] = moment($("#startTime").text(), "Do MMM YYYY H:mm").subtract(7, "days").format("YYYY-MM-DD HH:mm:ss");
        newReminder["end"] = moment($("#endTime").text(), "Do MMM YYYY H:mm").add(7, "days").format("YYYY-MM-DD HH:mm:ss");
    }
       updateReminder(newReminder);
}

//Ajax Request for Reminder
function updateReminder(json) {
    $.ajax({
        type: "GET",
        url: "/Reminder/Update",
        data: json,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (events) {
            console.log("Sucesso");
        },
        failure: function (response) {
            console.log("Fail");
            alert(response.d);
        }
    });


}
