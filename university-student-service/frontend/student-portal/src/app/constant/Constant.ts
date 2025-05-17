export const HOME_CONSTANTS = {
  TITLE: 'Welcome!',
  SCHEDULE_TITLE: 'MY SCHEDULE',
  NEXT_HEADING_LABEL: 'NEXT',
  COMING_UP_TITLE: 'COMING UP',
  NEWS_PANEL_TITLE: 'LATEST NEWS & EVENTS',
  WEEK_DAYS: ['M', 'T', 'W', 'T', 'F', 'S', 'S'],
  COMING_UP_FIRST_SESSION: '10:00 to 11:00 WZ203',
  COMING_UP_SESSIONS: [
    { time: '10:00 to 11:00 WZ203' },
    { time: '11:00 to 12:00 WZ405' },
    { time: '2:00 to 4:00 WZ702' },
    { time: '4:00 to 5:00 WZ805' },
  ],
  NEWS_ITEMS: [
    {
      img: '/newsitem1.png',
      alt: 'Student Group Fair',
      title: 'Student Group Fair',
      text: 'Discover student clubs and organisations, browse group activities, meet members and find your community on campus.',
      date: 'Thu, July 10, 2025',
    },
    {
      img: '/newsitem2.png',
      alt: 'Meet and Greet',
      title: 'International Student Meet & Greet',
      text: 'Kick off the semester by connecting with fellow students from around the world.',
      date: 'Wed, July 16, 2025',
    },
    {
      img: '/newsitem3.png',
      alt: 'Worldwide Cultural Festival',
      title: 'University Worldwide Cultural Festival',
      text: 'A celebration of dance, food and music from around the world.',
      date: 'Thu, July 24, 2025',
    },
  ],
};

export const ENROLMENT_TABLE_HEADERS = [
  'Period',
  'Class',
  'Class Name',
  'Status',
  'Start Date',
  'Location',
  'Level',
];

export const COURSE_INFO_TABLE_HEADERS = ['Code', 'Description', 'Points'];

export const LOGIN_API_URL = 'http://ec2-3-27-42-134.ap-southeast-2.compute.amazonaws.com:4999/api/Auth';
export const COURSE_API_URL : string = 'http://ec2-3-27-42-134.ap-southeast-2.compute.amazonaws.com:5000/api/course';
export const ENROLMENT_API_URL = 'http://ec2-3-27-42-134.ap-southeast-2.compute.amazonaws.com:5001/api/Enrolment';
export const STUDENT_DOCUMENTS_API_URL : string = 'http://ec2-3-27-42-134.ap-southeast-2.compute.amazonaws.com:5002/api/Doc';
export const STUDENT_API_URL : string = 'http://ec2-3-27-42-134.ap-southeast-2.compute.amazonaws.com:5003/api';
//export const STUDENT_API_URL : string = 'http://localhost:5001/api';
