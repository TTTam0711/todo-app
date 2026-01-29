window.calendarDragStart = function (event) {
    // 🔥 BẮT BUỘC cho Edge
    event.dataTransfer.setData("text/plain", "drag");
    event.dataTransfer.effectAllowed = "move";
};
