import './assets/main.css'
import 'primeicons/primeicons.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import router from '@/router'

import Aura from '@primeuix/themes/aura'
import PrimeVue from 'primevue/config'
import ConfirmationService from 'primevue/confirmationservice';
import DialogService from 'primevue/dialogservice';
import ToastService from 'primevue/toastservice';

import { definePreset } from '@primeuix/themes'
import moment from 'moment'
import 'moment/dist/locale/id'
import {
	Accordion,
	AccordionContent,
	AccordionHeader,
	AccordionPanel,
	AutoComplete,
	Avatar,
	AvatarGroup,
	Badge,
	BadgeDirective,
	BlockUI,
	Breadcrumb,
	Button,
	ButtonGroup,
	Card,
	Carousel,
	CascadeSelect,
	Checkbox,
	Chip,
	ColorPicker,
	Column,
	ColumnGroup,
	ConfirmDialog,
	ConfirmPopup,
	ContextMenu,
	DataTable,
	DataView,
	DatePicker,
	DeferredContent,
	Dialog,
	Divider,
	Dock,
	Drawer,
	DynamicDialog,
	Fieldset,
	FileUpload,
	FloatLabel,
	Fluid,
	Galleria,
	IconField,
	Image,
	Inplace,
	InputGroup,
	InputGroupAddon,
	InputIcon,
	InputMask,
	InputNumber,
	InputSwitch,
	InputText,
	Knob,
	Listbox,
	MegaMenu,
	Menu,
	Menubar,
	Message,
	MultiSelect,
	OrderList,
	OrganizationChart,
	OverlayBadge,
	Paginator,
	Panel,
	PanelMenu,
	Password,
	PickList,
	Popover,
	ProgressBar,
	ProgressSpinner,
	RadioButton,
	Rating,
	Ripple,
	Row,
	ScrollPanel,
	ScrollTop,
	Select,
	SelectButton,
	Skeleton,
	Slider,
	SpeedDial,
	SplitButton,
	Splitter,
	SplitterPanel,
	Step,
	StepList,
	StepPanel,
	StepPanels,
	Stepper,
	Steps,
	StyleClass,
	Tab,
	TabList,
	TabMenu,
	TabPanel,
	TabPanels,
	Tabs,
	Tag,
	Terminal,
	Textarea,
	TieredMenu,
	Timeline,
	Toast,
	ToggleButton,
	ToggleSwitch,
	Toolbar,
	Tooltip,
	Tree,
	TreeSelect,
	TreeTable,
	VirtualScroller
} from 'primevue';
import Chart from 'primevue/chart'

Date.prototype.toJSON = function() {
  return moment(this).format();
};

moment.locale('id')

const app = createApp(App);

app.directive('tooltip', Tooltip);
app.directive('badge', BadgeDirective);
app.directive('ripple', Ripple);
app.directive('styleclass', StyleClass);

app.component('Accordion', Accordion);
app.component('AccordionContent', AccordionContent);
app.component('AccordionHeader', AccordionHeader);
app.component('AccordionPanel', AccordionPanel);
// app.component('AccordionTab', AccordionTab);
app.component('AutoComplete', AutoComplete);
app.component('Avatar', Avatar);
app.component('AvatarGroup', AvatarGroup);
app.component('Badge', Badge);
app.component('BlockUI', BlockUI);
app.component('Breadcrumb', Breadcrumb);
app.component('Button', Button);
app.component('ButtonGroup', ButtonGroup);
// app.component('Calendar', Calendar);
app.component('Card', Card);
app.component('Chart', Chart);
app.component('Carousel', Carousel);
app.component('CascadeSelect', CascadeSelect);
app.component('Checkbox', Checkbox);
app.component('Chip', Chip);
// app.component('Chips', Chips);
app.component('ColorPicker', ColorPicker);
app.component('Column', Column);
app.component('ColumnGroup', ColumnGroup);
app.component('ConfirmDialog', ConfirmDialog);
app.component('ConfirmPopup', ConfirmPopup);
app.component('ContextMenu', ContextMenu);
app.component('DataTable', DataTable);
app.component('DataView', DataView);
app.component('DatePicker', DatePicker);
app.component('DeferredContent', DeferredContent);
app.component('Dialog', Dialog);
app.component('Divider', Divider);
app.component('Dock', Dock);
app.component('Drawer', Drawer);
// app.component('Dropdown', Dropdown);
app.component('DynamicDialog', DynamicDialog);
app.component('Fieldset', Fieldset);
app.component('FileUpload', FileUpload);
app.component('FloatLabel', FloatLabel);
app.component('Fluid', Fluid);
app.component('Galleria', Galleria);
app.component('IconField', IconField);
app.component('Image', Image);
// app.component('InlineMessage', InlineMessage);
app.component('Inplace', Inplace);
app.component('InputGroup', InputGroup);
app.component('InputGroupAddon', InputGroupAddon);
app.component('InputIcon', InputIcon);
app.component('InputMask', InputMask);
app.component('InputNumber', InputNumber);
app.component('InputSwitch', InputSwitch);
app.component('InputText', InputText);
app.component('Knob', Knob);
app.component('Listbox', Listbox);
app.component('MegaMenu', MegaMenu);
app.component('Menu', Menu);
app.component('Menubar', Menubar);
app.component('Message', Message);
app.component('MultiSelect', MultiSelect);
app.component('OrderList', OrderList);
app.component('OrganizationChart', OrganizationChart);
app.component('OverlayBadge', OverlayBadge);
// app.component('OverlayPanel', OverlayPanel);
app.component('Paginator', Paginator);
app.component('Panel', Panel);
app.component('PanelMenu', PanelMenu);
app.component('Password', Password);
app.component('PickList', PickList);
app.component('Popover', Popover);
app.component('ProgressBar', ProgressBar);
app.component('ProgressSpinner', ProgressSpinner);
app.component('RadioButton', RadioButton);
app.component('Rating', Rating);
app.component('Row', Row);
app.component('Select', Select);
app.component('SelectButton', SelectButton);
app.component('ScrollPanel', ScrollPanel);
app.component('ScrollTop', ScrollTop);
app.component('Slider', Slider);
// app.component('Sidebar', Sidebar);
app.component('Skeleton', Skeleton);
app.component('SpeedDial', SpeedDial);
app.component('SplitButton', SplitButton);
app.component('Splitter', Splitter);
app.component('SplitterPanel', SplitterPanel);
app.component('Step', Step);
app.component('Stepper', Stepper);
app.component('Steps', Steps);
app.component('StepList', StepList);
app.component('StepPanel', StepPanel);
app.component('StepPanels', StepPanels);
app.component('Tab', Tab);
app.component('Tabs', Tabs);
app.component('TabList', TabList);
app.component('TabMenu', TabMenu);
app.component('TabPanel', TabPanel);
app.component('TabPanels', TabPanels);
app.component('Tag', Tag);
app.component('Textarea', Textarea);
app.component('Terminal', Terminal);
app.component('TieredMenu', TieredMenu);
app.component('Timeline', Timeline);
app.component('Toast', Toast);
app.component('ToggleSwitch', ToggleSwitch);
app.component('Toolbar', Toolbar);
app.component('ToggleButton', ToggleButton);
app.component('Tree', Tree);
app.component('TreeSelect', TreeSelect);
app.component('TreeTable', TreeTable);
app.component('VirtualScroller', VirtualScroller);

app.use(createPinia())
app.use(router)
app.use(PrimeVue, {
  theme: {
    preset: Aura,
    options: {
			darkModeSelector: '.app-dark'
		}
  }
})
app.use(ToastService)
app.use(DialogService)
app.use(ConfirmationService)

app.mount('#app')
