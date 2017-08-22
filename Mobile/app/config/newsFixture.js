// news: novedades ("notificaciones") de sucesos que requieren atencion.
export const news = [
  {
    "type": "search", // permanent (until self-resolution?)
    "icon": "help",
    "message": {
      "started_by": "",
      "action": "Busqueda en curso por ti."
    }
  },
  {
    "type": "new_friend", // transient (until resolved: dismiss/accept/deny)
    "icon": "account-circle",
    "message": {
      "started_by": "Juan Perez",
      "action": "quiere agregarte."
    }
  },
  {
    "type": "find", // transient
    "icon": "help",
    "message": {
      "started_by": "Encuentrame",
      "action": "has visto a esta persona?"
    }
  }
];
