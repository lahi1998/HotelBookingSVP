import { Component } from '@angular/core';

@Component({
  selector: 'app-frontpage',
  standalone: false,
  templateUrl: './frontpage.html',
  styleUrl: './frontpage.css',
})
export class Frontpage {
  features = [
    {
      title: 'Komfortable værelser',
      text: 'Vores værelser kombinerer moderne design og klassisk elegance med udsigt over byen eller havet.',
      icon: '🛏️'
    },
    {
      title: 'Eksklusiv restaurant',
      text: 'Nyd lokale og internationale retter i vores prisvindende restaurant med fokus på friske råvarer.',
      icon: '🍽️'
    },
    {
      title: 'Spa & wellness',
      text: 'Forkæl dig selv i vores spa med sauna, massage og indendørs pool – perfekt til afslapning.',
      icon: '💆‍♀️'
    },
    {
      title: 'Arrangementer & møder',
      text: 'Vi tilbyder moderne mødelokaler og eventsale – ideelt til konferencer, bryllupper og fester.',
      icon: '🎉'
    }
  ];
}
