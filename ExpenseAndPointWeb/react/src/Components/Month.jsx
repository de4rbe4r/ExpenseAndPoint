import { React, useState, useEffect } from 'react';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { GetExpensesListByUserIdAndPeriodUrl, GetCategoryByIdUrl } from "../Urls/UrlList";
import Cookies from 'universal-cookie';
import AddCategoryBtn from './AddCategoryBtn'
import ExpenseForm from './ExpenseForm.jsx'
import ExpensesList from './ExpensesList.jsx';
import DayList from './DayList.jsx';
import AlertModal from './AlertModal.jsx';
import axios from 'axios';
import Button from 'react-bootstrap/Button';
import useWindowDimensions from '../Hooks/useWindowDimensions.jsx'


const Month = () => {
    const cookies = new Cookies();

    const { height, width } = useWindowDimensions();

    const [expensesDict, setExpensesDict] = useState({})
    const [totalAmountExpenses, setTotalAmountExpenses] = useState(null);
    const [expensesList, setExpensesList] = useState(undefined);
    const [fullExpensesList, setFullExpensesList] = useState(null);
    const [categoryList, setCategoryList] = useState([]);
    const [isDataUpdated, setDataUpdated] = useState(false);
    const [showForm, setShowForm] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [showAlertModal, setShowAlertModal] = useState(false);
    const [titleAlert, setTitleAlert] = useState("");
    const [pickedDate, setPickedDate] = useState();

    var today = new Date();
    var dateStart = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + "01";
    var dateEnd = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();

    const [userId, setUserId] = useState(cookies.get('userId'));

    useEffect(() => {
        axios.get(GetCategoryByIdUrl + userId)
            .then(res => {
                setCategoryList(res.data)
            })
            .catch(function (error) {
                if (error.response) {
                    setShowAlertModal(true);
                    setErrorMessage(error.message + ". " + error.response.data.detail);
                    setTitleAlert("Ошибка");
                }
            });
    }, [isDataUpdated]);

    useEffect(() => {
        axios.post(GetExpensesListByUserIdAndPeriodUrl, getRequest)
            .then(res => {
                setFullExpensesList(res.data);
                var temp = [{}];
                var totalAmount = 0;
                res.data.map((exp, index) => {
                    if (temp[exp.dateTime.substr(0, 10)] !== undefined) {
                        temp[exp.dateTime.substr(0, 10)].push(exp);
                    } else {
                        temp[exp.dateTime.substr(0, 10)] = [exp];
                    }
                    totalAmount += totalAmount + exp.amount;
                })
                setExpensesDict(temp);
                setTotalAmountExpenses(totalAmount);
                setExpensesList(expensesDict[pickedDate]);
            })
            .catch(function (error) {
                if (error.response) {
                    setErrorMessage(error.message + ". " + error.response.data.detail);
                }
            });
    }, [isDataUpdated]);

    var getRequest = {
        dateStart: dateStart,
        dateEnd: dateEnd,
        userId: userId,
    }

    const setIsShowForm = (data) => {
        setShowForm(data);
    }

    const setIsDataUpdated = (data) => {
        setDataUpdated(data);
    }

    const setIsShowAlertModal = (data) => {
        setShowAlertModal(data);
    }

    const openForm = (event) => {
        event.preventDefault();
        setShowForm(true);
    }

    const showExpensesListByDate = (date) => {
        setPickedDate(date);
        setExpensesList(expensesDict[date]);
    }

    if (fullExpensesList === null) return <img src="/loading.gif"></img>;

    return (
        <>
            <AlertModal showModal={showAlertModal} setIsShowModal={setIsShowAlertModal} errorMessage={errorMessage} title={titleAlert} />
            <Container>
                <ExpenseForm action="Добавить" setIsDataUpdated={setIsDataUpdated} categoryList={categoryList} expenseToEdit={undefined}
                    isShowForm={showForm} setIsShowForm={setIsShowForm} userId={userId} isDataUpdated={isDataUpdated} />
                <Row>
                    <Col sm={12}>
                        <Row>
                            <Col className="d-grid">
                                <Button variant="outline-light" size="lg" onClick={openForm}>Добавить расход</Button>
                            </Col>
                            <Col className="d-grid">
                                <AddCategoryBtn setIsDataUpdated={setIsDataUpdated} isDataUpdated={isDataUpdated} />
                            </Col>
                        </Row>
                        <Row>
                            <p></p>
                            <Col className="d-grid">
                            <Button variant="outline-light" size="lg" disabled>
                                    Итого за месяц { totalAmountExpenses}
                                </Button>
                            </Col>
                        </Row>
                        <Row>
                        <p></p>
                            <Col className="d-grid">
                                <DayList sendClickedDate={showExpensesListByDate} />
                            </Col>
                            <Col className="d-grid">
                                <ExpensesList expensesList={expensesList} categoryList={categoryList} userId={userId} setIsDataUpdatedToParent={setIsDataUpdated}
                                    isDataUpdated={isDataUpdated} height={0.88 * height} date={pickedDate} />
                            </Col>
                        </Row>
                    </Col>
                </Row>
            </Container>
        </>
    );
}

export default Month;