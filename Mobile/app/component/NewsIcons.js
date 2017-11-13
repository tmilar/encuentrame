import React from 'react';
import { Ionicons, EvilIcons, FontAwesome, MaterialCommunityIcons } from '@expo/vector-icons';

export const getIcon = (news) =>{
  switch(news.type) {
    case 'Areyouok.Ask':
      return <EvilIcons name='question' style={{ color: 'orange'}} size={40}/>;
      break;
    case 'Areyouok.Reply':
      return <Ionicons name='md-happy' style={{ color: 'green' }} size={40}/>;
      break;
    case 'Contact.Request':
      return <Ionicons name='md-contacts' style={{ color: 'green' }} size={40}/>;
      break;
    case 'Contact.Confirm':
      return <MaterialCommunityIcons name='account-check' style={{ color: 'black' }} size={40}/>;
      break;
    case 'Event/StartCollaborativeSearch':
      return <FontAwesome name='warning' size={40} style={{ color: 'red' }}/>;
      break;
    default:
      return <MaterialCommunityIcons name='alarm-light' style={{ color: 'black' }} size={40}/>;
  }
};
