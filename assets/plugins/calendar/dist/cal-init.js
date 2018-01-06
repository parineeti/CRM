
!function ($) {
    "use strict";

    var CalendarApp = function () {
        this.$body = $("body")
        this.$calendar = $('#calendar'),
        this.$event = ('#calendar-events div.calendar-events'),
        this.$categoryForm = $('#add-new-event form'),
        this.$extEvents = $('#calendar-events'),
        this.$modal = $('#my-event'),
         this.$addmodal = $('#add-new-event'),
        this.$saveCategoryBtn = $('#addSavebutton'),
        this.$eaddmodal = $('#eadd-new-event'),
        this.$esaveCategoryBtn = $('#eaddSavebutton'),
        this.$calendarObj = null
    };


    /* on drop */
    CalendarApp.prototype.onDrop = function (eventObj, date) {
        var $this = this;
        debugger;
        // retrieve the dropped element's stored Event Object
        var originalEventObject = eventObj.data('eventObject');
        var $categoryClass = eventObj.attr('data-class');
        // we need to copy it, so that multiple events don't have a reference to the same object
        var copiedEventObject = $.extend({}, originalEventObject);
        // assign it the date that was reported
        copiedEventObject.start = date;
        if ($categoryClass)
            copiedEventObject['className'] = [$categoryClass];
        // render the event on the calendar
        $this.$calendar.fullCalendar('renderEvent', copiedEventObject, true);
        // is the "remove after drop" checkbox checked?
        if ($('#drop-remove').is(':checked')) {
            // if so, remove the element from the "Draggable Events" list
            eventObj.remove();
        }
    },
    /* on click on event */
    CalendarApp.prototype.onEventClick = function (calEvent, jsEvent, view) {
        var $this = this;
        $("#eerrordiv").css("display", "none");
        var id = calEvent._id;
        var url = "/json/GetEventsById/";
        $.post(url, { 'id': id }, function (data) {
            var s1 = data["StartDate"];
            var s2 = data["EndDate"];
            var sp1 = s1.split(' ');
            var sp2 = s2.split(' ');
            $("#eid").val(id);
            $("#eName").val(data["Name"]);
            $("#ePoNumber").val(data["PoNumber"]);
            $("#eStartDate").val(sp1[0]);
            $("#eEndDate").val(sp2[0]);
            $("#eAssignTo").val(data["AssignTo"]);
            $("#eStatusId").val(data["StatusId"]);
            $("#eTypeId").val(data["TypeId"]);
            $("#edesc").val(data["Detail"]);
            if (data["IsActive"] == true) {
                $('#IsActive').prop('checked', true);
            } else {
                $('#IsActive').prop('checked', false);
            }
             $("#eshour").val(sp1[1].split(':')[0]);
              $("#esmin").val(sp1[1].split(':')[1]);
             $("#esampm").val(sp1[2]);
             $("#eehour").val(sp2[1].split(':')[0]);
              $("#eemin").val(sp2[1].split(':')[0]);
             $("#eeampm").val(sp2[2]);
            $this.$eaddmodal.modal({
                backdrop: 'static'
            });
        });
    },
    /* on select */
    CalendarApp.prototype.onSelect = function (start, end, allDay) {
        var $this = this;
        $this.$addmodal.modal({
            backdrop: 'static'
        });
        $("#errordiv").css("display", "none");
        $this.$calendarObj.fullCalendar('unselect');
    },
    CalendarApp.prototype.enableDrag = function () {
        //init events
        $(this.$event).each(function () {
            // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
            // it doesn't need to have a start or end
            var eventObject = {
                title: $.trim($(this).text()) // use the element's text as the event title
            };
            // store the Event Object in the DOM element so we can get to it later
            $(this).data('eventObject', eventObject);
            // make the event draggable using jQuery UI
            $(this).draggable({
                zIndex: 999,
                revert: true,      // will cause the event to go back to its
                revertDuration: 0  //  original position after the drag
            });
        });
    }
    /* Initializing */
    CalendarApp.prototype.init = function () {
        this.enableDrag();
        /*  Initialize the calendar  */
        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();
        var form = '';
        var today = new Date($.now());
        var $this = this;
        $this.$calendarObj = $this.$calendar.fullCalendar({

            defaultView: 'agendaWeek',
            handleWindowResize: true,
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'agendaWeek,month,agendaDay'
            },
            events: {
                url: '/json/GetEvents/',
                cache: true,
                type: 'GET',
                error: function () {
                    alert('there was an error while fetching events!');
                },
            },
             resizable: true,
            editable: true,
            droppable: true, // this allows things to be dropped onto the calendar !!!
            eventLimit: true, // allow "more" link when too many events
            selectable: true,
            drop: function (date) { $this.onDrop($(this), date); },
            select: function (start, end, allDay) { $this.onSelect(start, end, allDay); },
            eventClick: function (calEvent, jsEvent, view) {
                $this.onEventClick(calEvent, jsEvent, view);
            },
            eventDrop: function (event, delta, revertFunc) {
                alert(event.title + " was dropped on " + event.start.format("MM/DD/YYYY"));
                if (!confirm("Are you sure about this change?")) {
                    revertFunc();
                }else
                {   $.post("/json/UpdateEvent", { 'id': event.id, 'startDate': event.start.format("MM/DD/YYYY HH:mm"), 'EndDate': event.end.format("MM/DD/YYYY HH:mm") }, function () {

                    });
                }
            }
        });

        //on new event
        this.$saveCategoryBtn.on('click', function () {
            $("#errordiv").css("display", "block");
            $("#errortext").html("Please wait ,while we are creating job");
            var name = $("#Name").val();
            var poNumber = $("#PoNumber").val();
            var StartDate = $("#StartDate").val();
            var endDate = $("#EndDate").val();
            var AssignTo = $("#AssignTo").val();
            var StatusId = $("#StatusId").val();
            var TypeId = $("#TypeId").val();
            var desc = $("#desc").val();
            if (name == '' || poNumber == '' || StartDate == '' || endDate == '' || desc == '') {
                $("#errortext").html("Name ,PONumber,StartDate,EndDate And Decsription Can't be empty");
            }
            var shour = $("#shour").val();
            var smin = $("#smin").val();
            var sampm = $("#sampm").val();
            var ehour = $("#ehour").val();
            var emin = $("#emin").val();
            var eampm = $("#eampm").val();
            StartDate = StartDate + " " + shour + ":" + smin + ":00" + " " + sampm;
            endDate = endDate + " " + ehour + ":" + emin + ":00" + " " + eampm;          
            var url = "/job/createjob/";
            $.post(url, { 'TypeId': TypeId, 'StatusId': StatusId, 'AssignTo': AssignTo, 'name': name, 'poNumber': poNumber, 'StartDate': StartDate, 'EndDate': endDate, 'description': desc }, function (data) {
                if (data.indexOf("Error") == -1) {
                    $this.$calendarObj.fullCalendar('refetchEvents'); //the event has been removed from the database at this point so I just refetch the events
                    $("#Name").val('');
                    $("#PoNumber").val('');
                    $("#StartDate").val('');
                    $("#EndDate").val('');
                    $("#AssignTo").val('');
                    $("#StatusId").val('');
                    $("#TypeId").val('');
                    $("#desc").val('');
                    $this.$addmodal.modal('hide');
                }
                else {

                    $("#errortext").html(data);
                }
            });
        });

        this.$esaveCategoryBtn.on('click', function () {
            $("#eerrordiv").css("display", "block");
            $("#eerrortext").html("Please wait ,while we are updateing job");
            var name = $("#eName").val();
            var poNumber = $("#ePoNumber").val();
            var StartDate = $("#eStartDate").val();
            var endDate = $("#eEndDate").val();
            var AssignTo = $("#eAssignTo").val();
            var StatusId = $("#eStatusId").val();
            var TypeId = $("#eTypeId").val();
            var desc = $("#edesc").val();
            var id = $("#eid").val();
            var IsActive = $("#IsActive").prop('checked');
            if (name == '' || poNumber == '' || StartDate == '' || endDate == '' || desc == '') {
                $("#eerrortext").html("Name ,PONumber,StartDate,EndDate And Decsription Can't be empty");
            }
            var shour = $("#eshour").val();
            var smin = $("#esmin").val();
            var sampm = $("#esampm").val();
            var ehour = $("#eehour").val();
            var emin = $("#eemin").val();
            var eampm = $("#eeampm").val();
            StartDate = StartDate + " " + shour + ":" + smin + ":00" + " " + sampm;
            endDate = endDate + " " + ehour + ":" + emin + ":00" + " " + eampm;
            var url = "/job/updatejob/";
            $.post(url, { 'IsActive': IsActive, 'id': id, 'TypeId': TypeId, 'StatusId': StatusId, 'AssignTo': AssignTo, 'name': name, 'poNumber': poNumber, 'StartDate': StartDate, 'EndDate': endDate, 'description': desc }, function (data) {
                if (data.indexOf("Error") == -1) {
                    $this.$calendarObj.fullCalendar('refetchEvents'); //the event has been removed from the database at this point so I just refetch the events
                    $this.$eaddmodal.modal('hide');
                }
                else {
                    $("#eerrortext").html(data);
                }
            });
        });
    },

    //init CalendarApp
    $.CalendarApp = new CalendarApp, $.CalendarApp.Constructor = CalendarApp

}(window.jQuery),

//initializing CalendarApp
function ($) {
    "use strict";
    $.CalendarApp.init()
}(window.jQuery);