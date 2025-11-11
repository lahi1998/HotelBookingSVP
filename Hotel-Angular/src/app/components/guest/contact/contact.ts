import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-contact',
  standalone: false,
  templateUrl: './contact.html',
  styleUrl: './contact.css',
})
export class Contact {

  contactForm: FormGroup;
  submitted = false;


  constructor() {
    this.contactForm = new FormGroup({
      name: new FormControl('', [Validators.required, Validators.minLength(2)]),
      email: new FormControl('', [Validators.required, Validators.email]),
      phone: new FormControl('', [Validators.pattern('^[0-9+()\-\s]*$')]),
      subject: new FormControl('', [Validators.required]),
      message: new FormControl('', [Validators.required, Validators.minLength(10)])
    });
  }

  onSubmit(): void {
    this.submitted = true;


    if (this.contactForm.invalid) {
      return;
    }


    alert('Tak! Din besked er modtaget — vi vender tilbage hurtigst muligt.');
    this.contactForm.reset();
    this.submitted = false;
  }

}
